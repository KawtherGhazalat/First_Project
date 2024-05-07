using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public decimal? Userid { get; set; }

    public decimal? Categoryid { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<RecipeItem> RecipeItems { get; } = new List<RecipeItem>();

    public virtual User? User { get; set; }
}
