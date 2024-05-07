using System;
using System.Collections.Generic;

namespace First_Project.Models;

public partial class RecipeItem
{
    public decimal Recipeitemid { get; set; }

    public decimal? Recipeid { get; set; }

    public string Ingredient { get; set; } = null!;

    public string? Quantity { get; set; }

    public string? Unit { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
