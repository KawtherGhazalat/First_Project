using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace First_Project.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Items { get; set; } // Store items and quantities as JSON or XML
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Requester")]
        public string OrderRequester { get; set; }
        [NotMapped]
        public List<MenuItem> MenuItems { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        // Navigation properties
        public User User { get; set; }
        [Display(Name = "Approval")]
        public bool isPaid { get; set; }

    }
}
