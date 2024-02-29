using System;
using Microsoft.AspNetCore.Mvc;
using Miljobrott.Models;

namespace Miljobrott.Components
{
    public class ShowErrandDet : ViewComponent
    {
        private readonly IRepository repository;

        public ShowErrandDet(IRepository repo)
        {
            repository = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var errandDetail = await repository.GetErrandDet(id);

            return View(errandDetail);
        }
    }
}
