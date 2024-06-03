using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public partial class Profile
{
    [Key]
    public int ID { get; set; }
    [NotMapped]
    public int UserId { get; set; }
    public string? Image { get; set; }
    [DisplayName("Image")]
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public string? Bio { get; set; }
    public DateTime? CreationDate { get; set; }
    [NotMapped]
    [ForeignKey("UserId")]
    public virtual User? User { get; set; } = null!;
}

