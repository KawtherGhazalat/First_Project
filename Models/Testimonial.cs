using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal? Userid { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Dateposted { get; set; }
}
