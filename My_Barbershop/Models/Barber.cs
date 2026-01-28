using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Barbershop.Models;

public partial class Barber
{
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Display(Name = "Паспорт")]
    
    public long BarberPassport { get; set; }
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Display(Name = "ФИО")]
    public string? BarberFio { get; set; }
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Display(Name = "Телефон")]
    [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Неверный формат номера. Пример: +79261234567 или 89261234567")]
    public long? BarberPhnumber { get; set; }
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Display(Name = "Id Барбершопа")]
    [Range(1,3, ErrorMessage = "У нас всего три барбершопа")]
    public long? BNum { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Barbershop? BNumNavigation { get; set; }
    [NotMapped]
    public string PhotoUrl { get; set; } = "/images/barber1.jpg";
}
