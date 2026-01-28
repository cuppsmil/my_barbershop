using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Barbershop.Models;

public partial class Administrator
{
    [Display(Name = "Паспорт")]
    public long AdminPassport { get; set; }
    [Display(Name = "ФИО")]
    public string? AdmFio { get; set; }
    [Display(Name = "Номер телефона")]
    public long? AdminPhnumber { get; set; }
    [Display(Name = "Id барбершопа")]
    [Range(1, 3, ErrorMessage = "У нас всего три барбершопа")]
    public long? BNum { get; set; }

    public virtual Barbershop? BNumNavigation { get; set; }
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }

   
}
