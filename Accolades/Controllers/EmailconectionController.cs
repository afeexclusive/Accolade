using Accolades.ViewModels;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.Controllers
{
    public class EmailconectionController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailconectionController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: EmailconectionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmailconectionController/Details/5
        public ActionResult Details(ConnectionDetailsViewModel details)
        {
            return View(details);
        }

        // GET: EmailconectionController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: EmailconectionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ConnectionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(model.Domain))
                    {
                        var res = await _emailService.TestConnection(model.Email, model.Password);
                        return RedirectToAction(
                            "Details",
                            new ConnectionDetailsViewModel
                            {
                                IsAuthenticated = res.IsAuthenticated,
                                Domain = res.Domain,
                                Email = res.Email,
                                Password = res.Password,
                                Port = res.Port
                            });
                    }
                    else
                    {
                        var isAuth = await _emailService.TestConnection(model.Email, model.Password, model.Domain, model.Port);
                        return RedirectToAction("Details", new ConnectionDetailsViewModel { IsAuthenticated = isAuth});
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmailconectionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmailconectionController/Edit/5
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

        // GET: EmailconectionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmailconectionController/Delete/5
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
