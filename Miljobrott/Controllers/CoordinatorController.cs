using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Miljobrott.Models;
using Miljobrott.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Miljobrott.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {

        private readonly IRepository repository;
        public CoordinatorController(IRepository repo)
        {
            repository = repo;
        }
        public ViewResult CrimeCoordinator(int id)
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "samordnare";
            ViewBag.ID = id;
            TempData["ID"] = id;
            ViewBag.ListOfDepartments = repository.Departments;
            return View();
        }


        public IActionResult UpdateDepartment(string departmentId)
        {
            int EId = int.Parse(TempData["ID"].ToString());
            repository.UpdateDepartment(EId, departmentId);
            return RedirectToAction("CrimeCoordinator", new { id = EId });
        }

        public ViewResult ReportCrime()
        {
            var ReportedErrand = HttpContext.Session.Get<Errand>("ReportedErrand");
            if (ReportedErrand == null)
            {
                return View();
            }
            else
            {
                return View(ReportedErrand);
            }
        }
        public ViewResult StartCoordinator()
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "samordnare";

            return View(repository);
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
