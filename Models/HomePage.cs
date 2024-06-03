using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace First_Project.Models;

public partial class HomePage
{
    [Key]
    public int ID { get; set; }
    public string SectionName { get; set; } = null!;
    public string Content { get; set; } = null!;
}
