using System;
using System.Collections.Generic;

namespace My_Barbershop.Models;

public partial class Barberservice
{
    public long? Barbpass { get; set; }

    public string? Servname { get; set; }

    public virtual Barber? BarbpassNavigation { get; set; }

    public virtual Service? ServnameNavigation { get; set; }
}
