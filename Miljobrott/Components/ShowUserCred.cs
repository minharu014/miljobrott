using System;
using Microsoft.AspNetCore.Mvc;
using Miljobrott.Models;

namespace Miljobrott.Components
{
    public class ShowUserCred : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
