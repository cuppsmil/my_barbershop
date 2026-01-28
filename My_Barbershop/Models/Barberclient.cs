using System;
using System.Collections.Generic;

namespace My_Barbershop.Models;

public partial class Barberclient
{
    public long? Barbpass { get; set; }

    public long? Clientphone { get; set; }

    public virtual Barber? BarbpassNavigation { get; set; }

    public virtual Client? ClientphoneNavigation { get; set; }
}
