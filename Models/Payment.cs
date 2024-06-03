using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace First_Project.Models;

public class Payment
{
    [Key]
    public int ID { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public DateTime CreationDate { get; set; }
}
