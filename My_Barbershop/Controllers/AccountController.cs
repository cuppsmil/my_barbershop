using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Barbershop.Models;
using My_Barbershop.Models.ViewModels;
using System.Linq;

namespace My_Barbershop.Controllers
{
    public class AccountController : Controller
    {
        private readonly BarbershopContext _context;
        public AccountController(BarbershopContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            var barbershops = _context.Barbershops.ToList();
            ViewBag.Barbershops = barbershops;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Administrators.Any(a => a.AdminPassport == long.Parse(model.AdminPassport)))
                {
                    ModelState.AddModelError("AdminPassport", "Администратор с таким паспортом уже существует");
                    ViewBag.Barbershops = _context.Barbershops.ToList();
                    return View(model);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var admin = new Administrator
                {
                    AdminPassport = long.Parse(model.AdminPassport),
                    AdmFio = model.AdmFio,
                    AdminPhnumber = model.AdminPhnumber,
                    PasswordHash = hashedPassword,
                    BNum = model.BNum 
                };

                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            ViewBag.Barbershops = _context.Barbershops.ToList();
            return View(model);
        }

     
        public IActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterClient(RegisterClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (_context.Clients.Any(c => c.ClientPhnum == long.Parse(model.ClientPhnum)))
                {
                    ModelState.AddModelError("ClientPhnum", "Пользователь с таким номером телефона уже существует");
                    return View(model);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var client = new Client
                {
                    ClientPhnum = long.Parse(model.ClientPhnum),
                    ClFio = model.ClFio,
                    PasswordHash = hashedPassword
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string _passport, string password)
        {
            if (string.IsNullOrEmpty(_passport) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Введите логин и пароль");
                return View();
            }

           
            if (long.TryParse(_passport, out long passport))
            {
                var admin = await _context.Administrators
                    .FirstOrDefaultAsync(a => a.AdminPassport == passport);

                if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                {

                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToAction("Dashboard", "Admin"); 
                }
            }

           
            if (long.TryParse(_passport, out long phone))
            {
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.ClientPhnum == phone);

                if (client != null && BCrypt.Net.BCrypt.Verify(password, client.PasswordHash))
                {
                    
                    HttpContext.Session.SetString("Role", "Client");
                    HttpContext.Session.SetString("ClientPhone", phone.ToString());

                    return RedirectToAction("Profile", "Client");
                }
            }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }
    }
}