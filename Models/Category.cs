using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;
    [DisplayName("Image Path")]
    public string ImagePath { get; set; } = null!;
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public string Description { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
   
}
