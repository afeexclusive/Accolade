
using Application.Services.Interfaces;
using Application.ViewModel;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Application.Services.Implementations
{
    public class CertificateService : ICertificateService
    {
        public bool ManualGenerateAccoladeCertificate(CertificateVM model)
        {
            model.AwardName = $"{model.AwardName} in {model.AwardSubject}";

            try
            {
                int nameStartPossition = 500 - ((model.StudentName.Length / 2) * 25);
                int awardStartPostn = model.AwardName.Length > 30 ? 500 - ((model.AwardName.Length / 2) * 15) : 500 - ((model.AwardName.Length / 2) * 10);
                string formattedIssueDate = GetDateFormat(model.IssueDate);
                int monthPosition = model.AwardMonth.Length > 4 ? 520 : 500;
                string monthFolder = $"ACC_{model.AwardMonth}";


                 Directory.CreateDirectory($"./wwwroot/Accolade/{monthFolder}");


                var studentFileName = "";


                //string subdirectory = "Accolade";
                //string filename = "DesignTemplate.JPG";
                string accoladeTemplate = "./wwwroot/Accolade/DesignTemplate.JPG"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subdirectory, filename);
                Bitmap bmp;
                using (bmp = (Bitmap)Image.FromFile(accoladeTemplate))
                {
                    using (Graphics graphicsImage = Graphics.FromImage(bmp))
                    {
                        graphicsImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphicsImage.DrawString(model.StudentName, new Font("Verdana", 36, FontStyle.Bold), SystemBrushes.WindowText, new Point(nameStartPossition, 380));
                        graphicsImage.DrawString("for", new Font("Verdana", 26, FontStyle.Bold), SystemBrushes.WindowText, new Point(530, 460));
                        graphicsImage.DrawString(model.AwardName, new Font("Verdana", 20, FontStyle.Bold), SystemBrushes.WindowText, new Point(awardStartPostn, 520));
                        graphicsImage.DrawString($"({model.AwardMonth})", new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(monthPosition, 580));
                        graphicsImage.DrawString(formattedIssueDate, new Font("Calibri", 17, FontStyle.Bold), SystemBrushes.WindowText, new Point(50, 720));
                        
                    }

                    var tt = model.StudentName.Split(' ');
                    studentFileName = string.Join('_', tt);

                    bmp.Save($"./wwwroot/Accolade/{monthFolder}/{studentFileName}.jpg", ImageFormat.Jpeg);

                }
                var isCoverted = PdfHelper.ConvertCertificateToPDF($"./wwwroot/Accolade/{monthFolder}", $"{studentFileName}.jpg", $"./wwwroot/Accolade/{monthFolder}/{studentFileName}.pdf");

                //if (isCoverted)
                //{
                //    File.Delete($"./wwwroot/Accolade/{monthFolder}/{studentFileName}.jpg");
                //}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private string GetDateFormat(DateTimeOffset issueDate)
        {
            string day = issueDate.Day.ToString();
            string dayForm = "";

            if (day.Length > 1)
            {
                var endString = day.ToCharArray()[1].ToString();
                int lastDigit = int.Parse(endString);

                dayForm = $"{day}{GetDaySuffix(lastDigit)}";
            }
            else
            {
                dayForm = $"{day}{GetDaySuffix(int.Parse(day))}";
            }

            var monthInWords = issueDate.ToString("dd MMMM yy").Split(' ')[1];
            var year = issueDate.Year.ToString();

            return $"{dayForm} {monthInWords} {year}";

        }

        private string GetDaySuffix(int number)
        {
            string suff = "";
            switch (number)
            {
                case 1:
                    suff = "st";
                    break;
                case 2:
                    suff = "nd";
                    break;
                case 3:
                    suff = "rd";
                    break;
                default:
                    suff = "th";
                    break;
            }
            return suff;
        }
    }
}
