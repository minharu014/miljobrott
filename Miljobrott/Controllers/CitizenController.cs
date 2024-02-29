using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Miljobrott.Infrastructure;
using Miljobrott.Models;

namespace Miljobrott.Controllers
{
    public class CitizenController : Controller
    {
        private readonly IRepository repository;

        public CitizenController(IRepository repo)
        {
            repository = repo;
        }
        public ViewResult Faq()
        {
            return View();
        }
        public ViewResult Contact()
        {
            return View();
        }
        public ViewResult Services()
        {
            return View();
        }
        public ViewResult Thanks(Errand err)
        {
            err = HttpContext.Session.Get<Errand>("ReportedErrand");
            repository.SaveErrand(err);
            TempData["RefNumber"] = err.RefNumber;

            HttpContext.Session.Remove("ReportedErrand");
            return View();
        }

        [HttpPost]
        public ViewResult Validate(Errand err)
        {
            HttpContext.Session.Set<Errand>("ReportedErrand", err);
            return View(err);
        }
    }
}
