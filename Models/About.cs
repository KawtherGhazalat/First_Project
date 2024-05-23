using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class About
{
    public decimal Aboutid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}
