using First_Project.Models;

namespace First_Project.DTOs
{
    public class RecipeWithTestimonialsDTO
    {
        public Recipe Recipe { get; set; }
        public List<Testimonial> Testimonials { get; set; }

    }
}
