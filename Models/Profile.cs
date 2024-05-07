using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Profile
{
    public decimal Userid { get; set; }

    public string? Username { get; set; }

    public string? Rolename { get; set; }

    public string? Bio { get; set; }

    public virtual User User { get; set; } = null!;
}
