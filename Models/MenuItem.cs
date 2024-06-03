using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace First_Project.Models
{
    public class MenuItem
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Item name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        public int RecipeId { get; set; }
        [Required]
        [Display(Name = "Active")]
        public bool Status { get; set; } = true;
        public Category Category { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
