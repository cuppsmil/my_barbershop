using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Barbershop.Models;

public partial class Appointment
{
    [Key]
    public long Id { get; set; }
    [Required(ErrorMessage = "Обязательно ввести паспорт")]
    public long? Barberpassport { get; set; }
    [Required(ErrorMessage = "Обязательно ввести телефон")]
    public long? Clientphone { get; set; }

    public string? Servicename { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly Appointmentdate { get; set; }
    [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
    public TimeOnly Appointmenttime { get; set; }

    public virtual Barber? BarberpassportNavigation { get; set; }

    public virtual Client? ClientphoneNavigation { get; set; }

    public virtual Service? ServicenameNavigation { get; set; }
}
