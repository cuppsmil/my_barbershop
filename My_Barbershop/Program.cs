using Microsoft.EntityFrameworkCore;
using My_Barbershop.Models;

namespace My_Barbershop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddMvc();
            builder.Services.AddDbContext<BarbershopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information));
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(name: "Default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "account", pattern: "Account/{action}/{id?}");
            app.MapControllerRoute(name: "client_appointments", pattern: "Client/{action}/{id?}");
            app.Run();
        }
    }
}