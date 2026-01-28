using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace My_Barbershop.Models.ViewModels
{
    public class RegisterAdminViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Номер паспорта")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Номер паспорта должен быть от 10 до 12 символов")]
        public string AdminPassport { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "ФИО")]
        [StringLength(254, ErrorMessage = "Превышена максимальная длина (254 символа)")]
        public string AdmFio { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Неверный формат номера. Пример: +79261234567 или 89261234567")]
        public long? AdminPhnumber { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Выберите барбершоп")]
        [Display(Name = "Барбершоп")]
        public long BNum { get; set; }
    }
}
