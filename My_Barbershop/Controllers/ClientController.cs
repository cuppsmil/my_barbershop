using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Barbershop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace My_Barbershop.Controllers
{
    public class ClientController : Controller
    {
        private readonly BarbershopContext _context;

        public ClientController(BarbershopContext context)
        {
            _context = context;
        }
        public IActionResult Profile() { return View(); }

       
        [HttpGet]
        public async Task<IActionResult> BookAppointment()
        {
            var model = new Appointment
            {
                Appointmentdate = DateOnly.FromDateTime(DateTime.Today),
                Appointmenttime = TimeOnly.FromTimeSpan(new TimeSpan(10, 0, 0))
            };
          
            ViewBag.Barbershops = await _context.Barbershops.ToListAsync();
            ViewBag.Barbers = await _context.Barbers.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
              
                var clientExists = await _context.Clients
                    .AnyAsync(c => c.ClientPhnum == appointment.Clientphone);

                if (!clientExists)
                {
                    ModelState.AddModelError("Clientphone", "Клиент с таким номером не найден");
                    ViewBag.Barbershops = await _context.Barbershops.ToListAsync();
                    ViewBag.Barbers = await _context.Barbers.ToListAsync();
                    ViewBag.Services = await _context.Services.ToListAsync();
                    return View(appointment);
                }

              
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyAppointments");
            }

           
            ViewBag.Barbershops = await _context.Barbershops.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();
            return View(appointment);
        }

       

        [HttpGet]
        public async Task<IActionResult> MyAppointments()
        {
            var clientPhone = GetClientPhone();

            if (!clientPhone.HasValue)
            {
               
                return View(new List<Appointment>()); 
            }

            var appointments = await _context.Appointments
                .Include(a => a.BarberpassportNavigation)
                    .ThenInclude(b => b.BNumNavigation)
                .Include(a => a.ServicenameNavigation)
                .Where(a => a.Clientphone == clientPhone.Value)
                .ToListAsync();

            return View(appointments);
        }
       
        private long? GetClientPhone()
        {
            
            var phone = HttpContext.Session.GetString("ClientPhone");

            if (!string.IsNullOrEmpty(phone) && long.TryParse(phone, out var clientPhone))
            {
                return clientPhone;
            }

            
            var claim = User.FindFirst("ClientPhone");
            if (claim != null && long.TryParse(claim.Value, out var claimPhone))
            {
                return claimPhone;
            }

            return null;
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(long id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyAppointments");
        }

       
        [HttpGet]
        public async Task<IActionResult> ListServices()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }
    }
}