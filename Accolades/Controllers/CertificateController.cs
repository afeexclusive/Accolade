using Accolades.Models;
using Application.Services.Interfaces;
using Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.Controllers
{
    public class CertificateController : Controller
    {
        private readonly ICertificateService certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            this.certificateService = certificateService;
        }

        // GET: HomeController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // Post: Create
        [HttpPost]
        public ActionResult Create(CertificateViewModel model)
        {
            CertificateVM certificate = new CertificateVM()
            {
                AwardMonth = model.AwardMonth,
                AwardName = model.AwardName,
                AwardSubject = model.AwardSubject,
                IssueDate = model.IssueDate,
                StudentName = model.StudentName
            };

            certificateService.ManualGenerateAccoladeCertificate(certificate);
            return Redirect("Index");
        }

       

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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
