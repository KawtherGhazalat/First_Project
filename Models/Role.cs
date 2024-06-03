using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Role
{
    [Key]
    public int ID { get; set; }
    public string RoleName { get; set; } = null!;
    public virtual ICollection<User> Users { get; } = new List<User>();
}
