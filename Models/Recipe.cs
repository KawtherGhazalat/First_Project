using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Categoryid { get; set; }

    public string? Status { get; set; }
    [DisplayName("Image")]

    public string? Image { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }

}
