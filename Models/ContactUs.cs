using System.ComponentModel.DataAnnotations;

namespace First_Project.Models
{
    public class ContactUs
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
