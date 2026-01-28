using System.ComponentModel.DataAnnotations;

namespace My_Barbershop.Models.ViewModels
{
    public class ServiceViewModel
    {
        [Required(ErrorMessage = "Название услуги обязательно")]
        public string ServName { get; set; }
        [Required(ErrorMessage = "Сколько длится обязательно")]
        public double? Durofwork { get; set; }
        [Required(ErrorMessage = "Цена обязательно")]
        public double? ServPrice { get; set; }
        public int AppointmentCount { get; set; }
    }
}
