using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Barbershop.Models;
using My_Barbershop.Models.ViewModels;
using System.Linq;
using System.Text;
using System.Threading;

namespace My_Barbershop.Controllers
{
    public class AdminController : Controller
    {
        private readonly BarbershopContext _context;

        public AdminController(BarbershopContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        //
        // Барберы
        //
        public IActionResult BarberIndex(string sortField = "BarberFio", string sortOrder = "asc", string search = "")
        {
            var query = _context.Barbers
                .Include(b => b.BNumNavigation) 
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.BarberFio.Contains(search) || b.BarberPhnumber.ToString().Contains(search));
            }
            switch (sortField)
            {
                case "BarberPassport":
                    query = sortOrder == "asc"
                        ? query.OrderBy(b => b.BarberPassport)
                        : query.OrderByDescending(b => b.BarberPassport);
                    break;
                case "BarberFio":
                    query = sortOrder == "asc"
                        ? query.OrderBy(b => b.BarberFio)
                        : query.OrderByDescending(b => b.BarberFio);
                    break;
                case "BarberPhnumber":
                    query = sortOrder == "asc"
                        ? query.OrderBy(b => b.BarberPhnumber)
                        : query.OrderByDescending(b => b.BarberPhnumber);
                    break;
                case "Barbershop":  
                    query = sortOrder == "asc"
                        ? query.OrderBy(b => b.BNumNavigation.Name)
                        : query.OrderByDescending(b => b.BNumNavigation.Name);
                    break;
                default:
                    query = query.OrderBy(b => b.BarberFio); 
                    break;
            }
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Search = search;
            return View(query.ToList());
        }
        public IActionResult CreateBarber()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBarber(Barber barber)
        {
            if (ModelState.IsValid)
            {
                _context.Barbers.Add(barber);
                await _context.SaveChangesAsync();
                return RedirectToAction("BarberIndex");
            }
            return View(barber);
        }
        public async Task<IActionResult> EditBarber(long? id)
        {
            if (id == null) return NotFound();

            var barber = await _context.Barbers.FindAsync(id);
            if (barber == null) return NotFound();

           
            var barbershops = await _context.Barbershops.ToListAsync();

           
            ViewBag.Barbershops = barbershops ?? new List<Barbershop>(); 

            return View(barber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBarber(long id, Barber barber)
        {
            if (id != barber.BarberPassport) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(barber);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    if (!_context.Barbers.Any(b => b.BarberPassport == barber.BarberPassport))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("BarberIndex");
            }
            return View(barber);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBarber(long? id)
        {
            if (id == null) return NotFound();

            var barber = await _context.Barbers
                .Include(b => b.Appointments)
                .FirstOrDefaultAsync(b => b.BarberPassport == id);

            if (barber == null) return NotFound();

            
            ViewBag.HasAppointments = barber.Appointments != null && barber.Appointments.Any();
            ViewBag.AppointmentCount = barber.Appointments?.Count() ?? 0;

            return View(barber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBarberConfirmed(long id)
        {
            var barber = await _context.Barbers
                .Include(b => b.Appointments)
                .FirstOrDefaultAsync(b => b.BarberPassport == id);

            if (barber == null)
            {
                return NotFound();
            }

           
            if (barber.Appointments != null && barber.Appointments.Any())
            {
                ModelState.AddModelError("", "Нельзя удалить барбера, пока у него есть записи.");
                return View("DeleteBarber", barber);
            }

            try
            {
                var barberClients = await _context.Barberclients
           .Where(bc => bc.Barbpass == id)
           .ToListAsync();
                var barberServices = await _context.Barberservices
        .Where(bs => bs.Barbpass == id)
        .ToListAsync();

                if (barberServices.Count > 0)
                {
                    _context.Barberservices.RemoveRange(barberServices);
                    await _context.SaveChangesAsync(); 
                }

                if (barberClients.Count > 0)
                {
                    _context.Barberclients.RemoveRange(barberClients);
                    await _context.SaveChangesAsync();
                }
                _context.Barbers.Remove(barber);
                await _context.SaveChangesAsync();
                return RedirectToAction("BarberIndex");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении барбера: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
                return View("DeleteBarber", barber);
            }
        }
        //Удаление записей барбера
        public async Task<IActionResult> DeleteAppointments(long id)
        {
            var barber = await _context.Barbers
                .Include(b => b.Appointments)
                .FirstOrDefaultAsync(b => b.BarberPassport == id);

            if (barber == null || !barber.Appointments.Any())
            {
                return NotFound();
            }

            try
            {
                _context.Appointments.RemoveRange(barber.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteBarber", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении записей.");
                return View("DeleteBarber", barber);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointmentsConfirmed(long id)
        {
            var barber = await _context.Barbers
                .Include(b => b.Appointments)
                .FirstOrDefaultAsync(b => b.BarberPassport == id);

            if (barber == null || !barber.Appointments.Any())
            {
                return NotFound();
            }

            try
            {
                _context.Appointments.RemoveRange(barber.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteBarber", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении записей.");
                return View("DeleteAppointments", barber);
            }
        }
        //Экспорт барберов в таблицу Excel
        [HttpGet]
        public async Task<IActionResult> ExportBarberToExcel(string sortField = "BarberFio", string sortOrder = "asc", string search = "")
        {
            var query = _context.Barbers
                .Include(b => b.BNumNavigation)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.BarberFio.Contains(search) || b.BarberPhnumber.ToString().Contains(search));
            }
            var data = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Барберы");

            worksheet.Cell(1, 1).Value = "Паспорт";
            worksheet.Cell(1, 2).Value = "ФИО";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Барбершоп";

            for (int i = 0; i < data.Count; i++)
            {
                var barber = data[i];
                worksheet.Cell(i + 2, 1).Value = barber.BarberPassport;
                worksheet.Cell(i + 2, 2).Value = barber.BarberFio;
                worksheet.Cell(i + 2, 3).Value = barber.BarberPhnumber;
                worksheet.Cell(i + 2, 4).Value = barber.BNumNavigation?.Name ?? "Нет";
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"Barbers_{DateTime.Now:yyyy-MM-dd}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        //
        //Клиенты
        //
        public IActionResult ClientIndex(string sortField = "ClFio", string sortOrder = "asc", string search = "")
        {
            IQueryable<Client> query = _context.Clients.Include(c => c.Appointments); ;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.ClFio.Contains(search) || c.ClientPhnum.ToString().Contains(search));
            }
           
            switch (sortField)
            {
                case "ClFio":
                    query = sortOrder == "asc"
                        ? query.OrderBy(c => c.ClFio)
                        : query.OrderByDescending(c => c.ClFio);
                    break;
                case "ClientPhnum":
                    query = sortOrder == "asc"
                        ? query.OrderBy(c => c.ClientPhnum)
                        : query.OrderByDescending(c => c.ClientPhnum);
                    break;
                default:
                    query = query.OrderBy(c => c.ClFio);
                    break;
            }

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Search = search;
            return View(query);
        }
        public IActionResult CreateClient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("ClientIndex");
            }
            return View(client);
        }
        public async Task<IActionResult> EditClient(long? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClient(long id, Client client)
        {
            if (id != client.ClientPhnum) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    if (!_context.Clients.Any(c => c.ClientPhnum == client.ClientPhnum))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction("ClientIndex");
            }
            return View(client);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteClient(long? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.ClientPhnum == id);

            if (client == null) return NotFound();
            ViewBag.HasAppointments = client.Appointments != null && client.Appointments.Any();
            ViewBag.AppointmentCount = client.Appointments?.Count() ?? 0;

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClientConfirmed(long id)
        {
            var client = await _context.Clients
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.ClientPhnum == id);

            if (client == null)
            {
                return NotFound();
            }

           
            if (client.Appointments != null && client.Appointments.Any())
            {
                ModelState.AddModelError("", "Нельзя удалить клиента, пока у него есть записи.");
                return View("DeleteClient", client);
            }

            try
            {
                var barberClients = await _context.Barberclients
           .Where(bc => bc.Clientphone == id)
           .ToListAsync();

                if (barberClients.Count > 0)
                {
                    _context.Barberclients.RemoveRange(barberClients);
                    await _context.SaveChangesAsync();
                }
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("ClientIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении клиента: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
                return View("DeleteClient", client);
            }
        }
        //УДаление записей клиента
        [HttpGet]
        public async Task<IActionResult> DeleteClientAppointments(long? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.ClientPhnum == id);

            if (client == null || client.Appointments == null || !client.Appointments.Any())
            {
                return NotFound();
            }
            try
            {
                _context.Appointments.RemoveRange(client.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteClient", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении записей.");
                return View("DeleteClient", client);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClientAppointmentsConfirmed(long id)
        {
            var client = await _context.Clients
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.ClientPhnum == id);

            if (client == null || client.Appointments == null || !client.Appointments.Any())
            {
                return NotFound();
            }

            try
            {
                _context.Appointments.RemoveRange(client.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteClient", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении записей: {ex.Message}");
                return View("DeleteClientAppointments", client);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExportClientToExcel(string sortField = "ClFio", string sortOrder = "asc", string search = "")
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.ClFio.Contains(search) || c.ClientPhnum.ToString().Contains(search));
            }

            var data = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Клиенты");

            worksheet.Cell(1, 1).Value = "Телефон";
            worksheet.Cell(1, 2).Value = "ФИО";
            worksheet.Cell(1, 3).Value = "Записи";

            for (int i = 0; i < data.Count; i++)
            {
                var client = data[i];
                worksheet.Cell(i + 2, 1).Value = client.ClientPhnum;
                worksheet.Cell(i + 2, 2).Value = client.ClFio;
                worksheet.Cell(i + 2, 3).Value = client.Appointments?.Count() ?? 0;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"Clients_{DateTime.Now:yyyy-MM-dd}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        //
        //Услуги
        //
        [HttpGet]
        public IActionResult ServiceIndex(string sortField = "ServName", string sortOrder = "asc", string search = "")
        {
            IQueryable<Service> query = _context.Services.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.ServName.Contains(search));
            }
            switch (sortField)
            {
                case "ServName":
                    query = sortOrder == "asc"
                        ? query.OrderBy(s => s.ServName)
                        : query.OrderByDescending(s => s.ServName);
                    break;
                case "Durofwork":
                    query = sortOrder == "asc"
                        ? query.OrderBy(s => s.Durofwork)
                        : query.OrderByDescending(s => s.Durofwork);
                    break;
                case "ServPrice":
                    query = sortOrder == "asc"
                        ? query.OrderBy(s => s.ServPrice)
                        : query.OrderByDescending(s => s.ServPrice);
                    break;
            }

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Search = search;

            var result = query.Select(s => new ServiceViewModel
            {
                ServName = s.ServName,
                Durofwork = s.Durofwork,
                ServPrice = s.ServPrice,
                AppointmentCount = s.Appointments.Count()
            }).ToList();

            return View(result);
        }
        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (await _context.Services.AnyAsync(s => s.ServName == model.ServName))
                {
                    ModelState.AddModelError("ServName", "Услуга с таким названием уже существует");
                    return View(model);
                }

                var service = new Service
                {
                    ServName = model.ServName,
                    Durofwork = model.Durofwork,
                    ServPrice = model.ServPrice
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("ServiceIndex");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditService(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            var viewModel = new ServiceViewModel
            {
                ServName = service.ServName,
                Durofwork = service.Durofwork,
                ServPrice = service.ServPrice
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(string id, ServiceViewModel model)
        {
            if (id != model.ServName) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Services.FindAsync(model.ServName);
                    if (existing == null) return NotFound();

                    existing.Durofwork = model.Durofwork;
                    existing.ServPrice = model.ServPrice;

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при обновлении: " + ex.Message);
                    return View(model);
                }

                return RedirectToAction("ServiceIndex");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteService(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.ServName == id);

            if (service == null) return NotFound();

            
            ViewBag.HasAppointments = service.Appointments?.Any() ?? false;
            ViewBag.AppointmentCount = service.Appointments?.Count() ?? 0;

            var barberServices = await _context.Barberservices
                .Where(bs => bs.Servname == id)
                .ToListAsync();

            ViewBag.HasBarberServices = barberServices?.Any() ?? false;
            ViewBag.BarberServiceCount = barberServices.Count;

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteServiceConfirmed(string id)
        {
            var service = await _context.Services
                .Include(s => s.Appointments) 
                .FirstOrDefaultAsync(s => s.ServName == id);

            if (service == null)
            {
                return NotFound();
            }

           
            if (service.Appointments?.Any() ?? false)
            {
                ModelState.AddModelError("", "Нельзя удалить услугу, пока есть связанные записи.");
                return View("DeleteService", service);
            }

            try
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("ServiceIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении услуги: {ex.Message}");
                return View("DeleteService", service);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteServiceAppointments(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.ServName == id);

            if (service == null || service.Appointments == null || !service.Appointments.Any())
            {
                return NotFound();
            }
            try
            {
                _context.Appointments.RemoveRange(service.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteBarber", new { id });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении записей.");
                return View("DeleteService", service);
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteServiceAppointmentsConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.ServName == id);

            if (service == null || service.Appointments == null || !service.Appointments.Any())
            {
                return NotFound();
            }

            try
            {
                _context.Appointments.RemoveRange(service.Appointments);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeleteService", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении записей: {ex.Message}");
                return View("DeleteServiceAppointments", service);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExportServiceToExcel(string sortField = "ServName", string sortOrder = "asc", string search = "")
        {
            var query = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.ServName.Contains(search));
            }

            var data = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Услуги");

            worksheet.Cell(1, 1).Value = "Название";
            worksheet.Cell(1, 2).Value = "Длительность";
            worksheet.Cell(1, 3).Value = "Стоимость";

            for (int i = 0; i < data.Count; i++)
            {
                var service = data[i];
                worksheet.Cell(i + 2, 1).Value = service.ServName;
                worksheet.Cell(i + 2, 2).Value = service.Durofwork;
                worksheet.Cell(i + 2, 3).Value = service.ServPrice;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"Services_{DateTime.Now:yyyy-MM-dd}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        //
        //Записи
        //
        [HttpGet]
        public IActionResult AppointmentIndex(string sortField = "Appointmentdate", string sortOrder = "asc")
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.BarberpassportNavigation)
                .Include(a => a.ClientphoneNavigation)
                .Include(a => a.ServicenameNavigation);

           
            switch (sortField)
            {
                case "Appointmentdate":
                    query = sortOrder == "asc"
                        ? query.OrderBy(a => a.Appointmentdate)
                        : query.OrderByDescending(a => a.Appointmentdate);
                    break;
                case "Appointmenttime":
                    query = sortOrder == "asc"
                        ? query.OrderBy(a => a.Appointmenttime)
                        : query.OrderByDescending(a => a.Appointmenttime);
                    break;
                case "Barber":
                    query = sortOrder == "asc"
                        ? query.OrderBy(a => a.BarberpassportNavigation.BarberFio)
                        : query.OrderByDescending(a => a.BarberpassportNavigation.BarberFio);
                    break;
                case "Client":
                    query = sortOrder == "asc"
                        ? query.OrderBy(a => a.ClientphoneNavigation.ClFio)
                        : query.OrderByDescending(a => a.ClientphoneNavigation.ClFio);
                    break;
                case "Service":
                    query = sortOrder == "asc"
                        ? query.OrderBy(a => a.ServicenameNavigation.ServName)
                        : query.OrderByDescending(a => a.ServicenameNavigation.ServName);
                    break;
            }

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            var result = query.Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                Appointmentdate = a.Appointmentdate,
                Appointmenttime = a.Appointmenttime,
                Barberpassport = a.Barberpassport,
                Clientphone = a.Clientphone,
                Servicename = a.Servicename,
                BarberFio = a.BarberpassportNavigation.BarberFio,
                ClientFio = a.ClientphoneNavigation.ClFio,
                ServiceName = a.ServicenameNavigation.ServName
            }).ToList();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> CreateAppointment()
        {
            ViewBag.Barbers = await _context.Barbers.ToListAsync();
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    Appointmentdate = model.Appointmentdate,
                    Appointmenttime = model.Appointmenttime,
                    Barberpassport = model.Barberpassport,
                    Clientphone = model.Clientphone,
                    Servicename = model.Servicename
                };

                _context.Appointments.Add(appointment);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AppointmentIndex");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ошибка при добавлении записи: " + ex.InnerException?.Message);
                    ViewBag.Barbers = await _context.Barbers.ToListAsync();
                    ViewBag.Clients = await _context.Clients.ToListAsync();
                    ViewBag.Services = await _context.Services.ToListAsync();
                    return View(model);
                }
            }

           
            ViewBag.Barbers = await _context.Barbers.ToListAsync();
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditAppointment(long? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.BarberpassportNavigation)
                .Include(a => a.ClientphoneNavigation)
                .Include(a => a.ServicenameNavigation)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            var viewModel = new AppointmentViewModel
            {
                Id = appointment.Id,
                Appointmentdate = appointment.Appointmentdate,
                Appointmenttime = appointment.Appointmenttime,
                Barberpassport = appointment.Barberpassport,
                Clientphone = appointment.Clientphone,
                Servicename = appointment.Servicename,
                BarberFio = appointment.BarberpassportNavigation?.BarberFio,
                ClientFio = appointment.ClientphoneNavigation?.ClFio,
                ServiceName = appointment.ServicenameNavigation?.ServName
            };

            
            ViewBag.Barbers = await _context.Barbers.ToListAsync();
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(long id, AppointmentViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Appointments
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (existing == null) return NotFound();

                   
                    existing.Appointmentdate = model.Appointmentdate;
                    existing.Appointmenttime = model.Appointmenttime;
                    existing.Barberpassport = model.Barberpassport;
                    existing.Clientphone = model.Clientphone;
                    existing.Servicename = model.Servicename;

                    _context.Appointments.Update(existing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AppointmentIndex");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при обновлении: " + ex.Message);
                }
            }

           
            ViewBag.Barbers = await _context.Barbers.ToListAsync();
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Services = await _context.Services.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAppointment(long? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.BarberpassportNavigation)
                .Include(a => a.ClientphoneNavigation)
                .Include(a => a.ServicenameNavigation)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            var viewModel = new AppointmentViewModel
            {
                Id = appointment.Id,
                Appointmentdate = appointment.Appointmentdate,
                Appointmenttime = appointment.Appointmenttime,
                BarberFio = appointment.BarberpassportNavigation?.BarberFio,
                ClientFio = appointment.ClientphoneNavigation?.ClFio,
                ServiceName = appointment.ServicenameNavigation?.ServName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointmentConfirmed(long id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            try
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("AppointmentIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка при удалении записи: " + ex.Message);
                return View("DeleteAppointment", appointment);
            }
        }
       
       
       
        [HttpPost]
        public async Task<IActionResult> ExportReport(string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
            {
                ModelState.AddModelError("", "Неверный формат даты");
                return RedirectToAction("MyAppointments");
            }

           
            var appointments = await _context.Appointments
                .Include(a => a.BarberpassportNavigation)
                .ThenInclude(b => b.BNumNavigation)
                .Include(a => a.ClientphoneNavigation)
                .Include(a => a.ServicenameNavigation)
                .Where(a => a.Appointmentdate >= DateOnly.FromDateTime(start) &&
                            a.Appointmentdate <= DateOnly.FromDateTime(end))
                .ToListAsync();

            if (!appointments.Any())
            {
                ModelState.AddModelError("", "Записей за выбранный период не найдено");
                return RedirectToAction("MyAppointments");
            }

           
            var report = new StringBuilder();
            report.AppendLine($"Отчет за период: {start.ToString("yyyy-MM-dd")} - {end.ToString("yyyy-MM-dd")}\n\n");

            foreach (var app in appointments)
            {
                report.AppendLine($"Запись ID: {app.Id}");
                report.AppendLine($"Дата: {app.Appointmentdate}");
                report.AppendLine($"Время: {app.Appointmenttime}\n");

               
                if (app.ClientphoneNavigation != null)
                {
                    report.AppendLine("Клиент:");
                    report.AppendLine($"Телефон: {app.ClientphoneNavigation.ClientPhnum}");
                    report.AppendLine($"ФИО: {app.ClientphoneNavigation.ClFio}\n");
                }

              
                if (app.BarberpassportNavigation != null)
                {
                    report.AppendLine("Барбер:");
                    report.AppendLine($"Паспорт: {app.BarberpassportNavigation.BarberPassport}");
                    report.AppendLine($"ФИО: {app.BarberpassportNavigation.BarberFio}");
                    report.AppendLine($"Телефон: {app.BarberpassportNavigation.BarberPhnumber}\n");

                    if (app.BarberpassportNavigation.BNumNavigation != null)
                    {
                        report.AppendLine("Барбершоп:");
                        report.AppendLine($"Название: {app.BarberpassportNavigation.BNumNavigation.Name}");
                        report.AppendLine($"Адрес: {app.BarberpassportNavigation.BNumNavigation.Address}\n");
                    }
                }

               
                if (app.ServicenameNavigation != null)
                {
                    report.AppendLine("Услуга:");
                    report.AppendLine($"Название: {app.ServicenameNavigation.ServName}");
                    report.AppendLine($"Стоимость: {app.ServicenameNavigation.ServPrice} ₽");
                    report.AppendLine($"Длительность: {app.ServicenameNavigation.Durofwork} мин.\n");
                }

                report.AppendLine(new string('-', 60)); 
            }

            
            return File(
                Encoding.UTF8.GetBytes(report.ToString()),
                "text/plain",
                $"Report_{start.ToString("yyyyMMdd")}_{end.ToString("yyyyMMdd")}.txt"
            );
        }

    }
}