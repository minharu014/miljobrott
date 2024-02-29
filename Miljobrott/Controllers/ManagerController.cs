using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miljobrott.Models;

namespace Miljobrott.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IRepository repository;
        public ManagerController(IRepository repo)
        {
            repository = repo;
        }
        public ViewResult CrimeManager(int id)
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "avdelningschef";
            ViewBag.ListOfEmployees = repository.GetDepartmentEmp();
            ViewBag.ID = id;
            TempData["ID"] = id;
            return View();
        }

        public IActionResult UpdateManager(string employeeId, bool noAction, string reason)
        {
            int EId = int.Parse(TempData["ID"].ToString());
            repository.UpdateManager(EId, employeeId, noAction, reason);

            return RedirectToAction("CrimeManager", new { id = EId });
        }

        public ViewResult StartManager()
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "avdelningschef";

            return View(repository);
        }

    }
}
