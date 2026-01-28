using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace My_Barbershop.Models.ViewModels
{
   
        public class RegisterClientViewModel
        {
            [Required(ErrorMessage = "Поле обязательно для заполнения")]
            [Display(Name = "Номер телефона")]
            [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Неверный формат номера. Пример: +79261234567 или 89261234567")]
        public string ClientPhnum { get; set; }

            [Required(ErrorMessage = "Поле обязательно для заполнения")]
            [Display(Name = "ФИО")]
            [StringLength(254, ErrorMessage = "Превышена максимальная длина (254 символа)")]
            public string ClFio { get; set; }

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
        }
    
}
