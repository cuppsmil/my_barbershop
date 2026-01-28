using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Barbershop.Models;

public partial class Client
{
    [Display(Name = "Телефон")]
    [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Неверный формат номера. Пример: +79261234567 или 89261234567")]
    public long ClientPhnum { get; set; }
    [Display(Name = "ФИО")]
    [Required(ErrorMessage = "ФИО обязательно")]
    [StringLength(254)]
    public string? ClFio { get; set; }

    public string? DiscountCard { get; set; }

    public string? PasswordHash { get; set; } = null!;

    public DateTime? CardIssueDate { get; set; }

    [Range(0, 10000, ErrorMessage = "Бонусные баллы должны быть между 0 и 10000")]
    public int? BonusPoints { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    [NotMapped]
    public int AppointmentCount => Appointments?.Count() ?? 0;
}
    
