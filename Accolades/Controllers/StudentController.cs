using Accolades.ViewModels;
using AccoladesData.Entities;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
