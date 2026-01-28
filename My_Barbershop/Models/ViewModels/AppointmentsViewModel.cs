using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using My_Barbershop.Models;

namespace My_Barbershop.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Выберите дату")]
        [Display(Name = "Дата записи")]
        public DateOnly Appointmentdate { get; set; }

        [Required(ErrorMessage = "Введите время")]
        [Display(Name = "Время записи")]
        public TimeOnly Appointmenttime { get; set; }

        [Required(ErrorMessage = "Выберите барбера")]
        [Display(Name = "Барбер")]
        public long? Barberpassport { get; set; }

        [Required(ErrorMessage = "Выберите клиента")]
        [Display(Name = "Клиент")]
        public long? Clientphone { get; set; }

        [Required(ErrorMessage = "Выберите услугу")]
        [Display(Name = "Услуга")]
        public string? Servicename { get; set; }

       
        public string? BarberFio { get; set; }
        public string? ClientFio { get; set; }
        public string? ServiceName { get; set; }
    }
}
