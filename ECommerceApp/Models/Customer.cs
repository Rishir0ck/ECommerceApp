using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Models;

[Index(nameof(Email),Name = "IX_Email_Unique", IsUnique = true)]
public class Customer
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Please enter your first name")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your last name")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Please enter your email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please enter your phone number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please enter your Date Of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<Address>? Addresses { get; set; }
    public ICollection<Cart>? Carts { get; set; }
    public ICollection<Feedback>? Feedbacks { get; set; }
}
