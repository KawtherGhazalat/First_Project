using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal? Roleid { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; } = new List<Testimonial>();

    public static implicit operator decimal(User v)
    {
        throw new NotImplementedException();
    }
}
