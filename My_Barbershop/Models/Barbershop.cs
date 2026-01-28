using System;
using System.Collections.Generic;

namespace My_Barbershop.Models;

public partial class Barbershop
{
    public long Id { get; set; }

    public string? Address { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Administrator> Administrators { get; set; } = new List<Administrator>();

    public virtual ICollection<Barber> Barbers { get; set; } = new List<Barber>();
}
