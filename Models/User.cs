using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class User
{
    [Key]
    public int ID { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int ProfileId { get; set; }
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    [ForeignKey("ProfileId")]
    public virtual Profile? Profile { get; set; }
    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }
}
