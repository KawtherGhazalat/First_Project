namespace First_Project.DTOs
{
    public class RecipeReportDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Instructions { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Username { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
