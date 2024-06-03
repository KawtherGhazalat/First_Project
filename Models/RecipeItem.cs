using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
namespace First_Project.Models;

public partial class RecipeItem
{
    [Key]
    public int ID { get; set; }

    public decimal Recipeitemid { get; set; }

    public decimal? Recipeid { get; set; }

    public string Ingredient { get; set; } = null!;

    public string? Quantity { get; set; }

    public string? Unit { get; set; }
}
