using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Recipe
{
    [Key]
    public int ID { get; set; }

    public decimal RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public string Instructions { get; set; } = null!;

    public decimal Price { get; set; }

    public int UserId { get; set; }
    public bool isSold{ get; set; }

    public int CategoryId { get; set; }

    public string? Status { get; set; }
    [DisplayName("Image")]

    public string? Image { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

}
