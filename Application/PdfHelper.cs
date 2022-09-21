
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Application
{
    public static class PdfHelper
    {
        public static bool ConvertCertificateToPDF(string imageDirectory, string imageName, string pdfPath)
        {
            try
            {

                string imagePath = imageDirectory + "/" + imageName;

                PdfHelp.Instance.SaveImageAsPdf(imagePath, pdfPath, 900, true);
                

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
    }


    public sealed class PdfHelp
    {
        private PdfHelp()
        {
        }

        public static PdfHelp Instance { get; } = new PdfHelp();

        internal void SaveImageAsPdf(string imageFileName, string pdfFileName, int width = 900, bool deleteImage = false)
        {
            using (var document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                using (XImage img = XImage.FromFile(imageFileName))
                {
                    // Calculate new height to keep image ratio
                    var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                    // Change PDF Page size to match image
                    page.Width = width;
                    page.Height = height;

                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(img, 0, 0, width, height);
                }
                document.Save(pdfFileName);
            }

            if (deleteImage)
                File.Delete(imageFileName);
        }
    }
}


//(string ImagePath, string pdfPath)
//Document document = new Document();
//Page page = new Page();
//document.Pages.Add(page);
//Image image = new Image(ImagePath, 0, 0);
//page.Elements.Add(image);
//document.Draw(pdfPath);
//return true;

//(string imageDirectory, string imageName, string pdfPath)
//var imageFile = Directory.EnumerateFiles(imageDirectory).Where(f => f.EndsWith(imageName)).FirstOrDefault();
//ImageToPdfConverter.ImageToPdf(imageFile, ImageBehavior.FitToPage).SaveAs(pdfPath);
//return true;


//File(stream.ToArray(), "application/pdf", "CoreProgramm_Image_PDF_Converter.pdf");


//using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
//{

//    MemoryStream stream = new MemoryStream();

//    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
//    pdfDoc.Open();

//    //Add the Image file to the PDF document object.
//    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
//    var newpage = pdfDoc.NewPage();

//    var added = pdfDoc.Add(img);

//    FileStream outputFileStream = new FileStream(pdfPath, FileMode.Create);
//    stream.CopyTo(outputFileStream);


//    pdfDoc.Close();
//}