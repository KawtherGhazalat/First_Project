using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal? Userid { get; set; }

    public decimal Amount { get; set; }

    public DateTime Paymentdate { get; set; }
}
