using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class HomePage
{
    public decimal Sectionid { get; set; }

    public string Sectionname { get; set; } = null!;

    public string Content { get; set; } = null!;
}
