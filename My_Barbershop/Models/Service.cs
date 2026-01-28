using System;
using System.Collections.Generic;

namespace My_Barbershop.Models;

public partial class Service
{
    public string ServName { get; set; } = null!;

    public double? Durofwork { get; set; }

    public double? ServPrice { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
