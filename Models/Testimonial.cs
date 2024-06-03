using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Testimonial
{
    [Key]
    public int ID { get; set; }

    public int? UserId { get; set; }
    public int? RecipeId { get; set; }
    public bool isActive{ get; set; }

    public string Content { get; set; } = null!;
    [ForeignKey("UserId")]
    public User? User { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe? Recipe { get; set; }

    public DateTime? Dateposted { get; set; }
}
