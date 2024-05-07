using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
