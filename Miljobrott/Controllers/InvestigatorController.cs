using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Miljobrott.Models;

namespace Miljobrott.Controllers
{
    [Authorize(Roles = "Investigator")]
    public class InvestigatorController : Controller
    {

        private readonly IRepository repository;
        private IWebHostEnvironment environment;
        public InvestigatorController(IRepository repo, IWebHostEnvironment env)
        {
            repository = repo;
            environment = env;
        }

        public ViewResult CrimeInvestigator(int id)
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "handläggare";
            ViewBag.ID = id;
            TempData["ID"] = id; // så jag kan göra det till sträng
            ViewBag.ListOfStatuses = repository.ErrandStatuses;
            return View();
        }

     
        public async Task<IActionResult> UpdateInvestigator(string events, string information, string StatusId, IFormFile loadSample, IFormFile loadImage)
        {
            int eId = int.Parse(TempData["ID"].ToString());

            if (loadSample != null)
            {
                await FileManager(eId, loadSample, "application/pdf");
            }

            if (loadImage != null)
            {
                var contentType = loadImage.ContentType.ToLower();
                if (contentType.Contains("image/jpeg") || contentType.Contains("image/png") || contentType.Contains("image/jpg"))
                {
                    await FileManager(eId, loadImage, contentType);
                }
            }


            repository.UpdateInvestigator(eId, events, information, StatusId);

            return RedirectToAction("CrimeInvestigator", new { id = eId });
        }

        // filemanager för att lägga till en fil, pdf och jpeg. PNG funkar inte
        private async Task FileManager(int id, IFormFile file, string contentType)
        {
            if (file.Length > 0 && (contentType == "application/pdf" || contentType == "image/jpeg"))
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileName = Guid.NewGuid() + "_" + file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", fileName);

                System.IO.File.Move(filePath, path);

                if (contentType == "application/pdf")
                {
                    repository.SaveSample(fileName, id);
                }
                else if (contentType == "image/jpeg" || contentType == "image/png" || contentType == "image/jpg")
                {
                    repository.SaveImage(fileName, id);
                }
            }
        }


        public ViewResult StartInvestigator()
        {
            ViewBag.UserCredentials = repository.GetNameAndLogin();
            ViewBag.UserRoles = "handläggare";

            return View(repository);
        }
    }
}
