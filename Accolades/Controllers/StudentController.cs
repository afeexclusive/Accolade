using Accolades.ViewModels;
using AccoladesData.Entities;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.Controllers
{
    public class StudentController : Controller
    {
 
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            
            this.studentService = studentService;
            this.studentService.UserId = Guid.Parse(PersistedUser.UserId);
        }

        // GET: StudentController
        public ActionResult Index()
        {
            var model = studentService.GetAllStudents(PersistedUser.UserId);
            return View(model);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateStudentEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentEmail(IFormFile file)
        {
            string userID = PersistedUser.UserId;
            try
            {
                var headingList = await studentService.CreateEmailMap(file, userID);

                return RedirectToAction("EmailMapping");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateCorperDefaultList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCorperDefaultList(IFormFile file)
        {
            try
            {
                var recordds = studentService.UploadCorperData(file);
                ViewBag.corperData = recordds;
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> DownloadExcel() //List<CorpMemberDefualtData> model
        {
            var req = Request.Form["model"];
            var model = JsonConvert.DeserializeObject<List<CorpMemberDefualtData>>(req);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                // setting the properties
                // of the work sheet 
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;

                // Setting the properties
                // of the first row
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;

                // Header of the Excel sheet
                workSheet.Cells[1, 1].Value = "State Code";
                workSheet.Cells[1, 2].Value = "Surname";
                workSheet.Cells[1, 3].Value = "Other Names";
                workSheet.Cells[1, 4].Value = "Phone Number";
                workSheet.Cells[1, 5].Value = "LGA";
                workSheet.Cells[1, 6].Value = "Months";

                // Inserting the article data into excel sheet by using the for each loop // As we have values to the first row  // we will start with second row
                int recordIndex = 2;

                foreach (var item in model)
                {
                    workSheet.Cells[recordIndex, 1].Value = item.StateCode;
                    workSheet.Cells[recordIndex, 2].Value = item.LastName;
                    workSheet.Cells[recordIndex, 3].Value = item.OtherNames;
                    workSheet.Cells[recordIndex, 4].Value = item.PhoneNumber;
                    workSheet.Cells[recordIndex, 5].Value = item.LGA;
                    workSheet.Cells[recordIndex, 6].Value = item.Months;
                    recordIndex++;
                }

                // By default, the column width is not set to auto fit for the content of the range, so we are using AutoFit() method here. 
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();

                // file name with .xlsx extension 
                string p_strPath = $"./Data/defualters.xlsx";

                if (System.IO.File.Exists(p_strPath))
                    System.IO.File.Delete(p_strPath);
                
                 // Create excel file on physical disk 
                 FileStream objFileStrm = System.IO.File.Create(p_strPath);
                objFileStrm.Close();

                // Write content to excel file 
                System.IO.File.WriteAllBytes(p_strPath, excel.GetAsByteArray());


                //Close Excel package
                excel.Dispose();

                byte[] filedata = System.IO.File.ReadAllBytes(p_strPath);

                FileInfo fi = new FileInfo(p_strPath);
                
                return File(filedata, "application/force-download", fi.Name);
            }


        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        [HttpGet]
        public ActionResult CreateAccolade()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccolade(IFormFile file)
        {
            string userID = PersistedUser.UserId;
            try
            {
                var headingList = await studentService.ReadAccolades(file, userID);

                return RedirectToAction("EmailMapping");
            }
            catch
            {
                return View();
            }
        }






        [HttpGet]
        public ActionResult EmailMapping()
        {
            var emailHeaderModel = studentService.GetEmailHeaders(PersistedUser.UserId);
            ViewBag.Headings = emailHeaderModel.Select(x => x.Heading).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EmailMapping(StudentsViewModel model)
        {
            //string userId = ""
            var student = new Student
            {
                DateOfBirth = model.DateOfBirth,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                FileName = model.FileName
            };
            studentService.UpdateStudentEmail(student, PersistedUser.UserId);

            model.FileName = null;

            return RedirectToAction("Index");
        }

        // GET: StudentController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile file)
        {
            string userID = PersistedUser.UserId;
            try
            {
               var headingList = await studentService.CreateMapHeadings(file, userID);

                return RedirectToAction("Mapping");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Mapping()
        {
            ViewBag.Headings = studentService.GetAllHeaders(PersistedUser.UserId);
            return View();
        }

        [HttpPost]
        public ActionResult Mapping(StudentsViewModel model)
        {
            //string userId = ""
            var student = new Student
            {
                AdmissionNumber = model.AdmissionNumber,
                Class = model.Class,
                FatherEmail = model.FatherEmail,
                FatherPhone = model.FatherPhone,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                MiddeleName = model.MiddeleName,
                MotherPhone = model.MotherPhone,
                MotherEmail = model.MotherEmail,
                ParentName = model.ParentName,
                 FileName = model.FileName
            };
            studentService.UploadStudentData(student, PersistedUser.UserId);

            model.FileName = null;

            return RedirectToAction("Index");
        }

        // GET: StudentController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
