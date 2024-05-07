using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
}
