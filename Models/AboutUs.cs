using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace First_Project.Models;

public partial class AboutUs
{
    [Key]
    public int ID { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}
