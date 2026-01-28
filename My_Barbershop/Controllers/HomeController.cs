using Microsoft.AspNetCore.Mvc;
using System;

namespace My_Barbershop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (role == "Client")
            {
                return RedirectToAction("Profile", "Client");
            }

            return View();
        }
    }
}