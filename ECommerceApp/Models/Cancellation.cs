using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models;

public class Cancellation
{
    public int Id { get; set; }
        // Foreign key to Order
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        // Reason for cancellation
        [Required(ErrorMessage = "Cancellation reason is required.")]
        [StringLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters.")]
        public string? Reason { get; set; }
        // Status of the cancellation request
        [Required]
        public CancellationStatus Status { get; set; }
        // Date and time when the cancellation was requested
        [Required]
        public DateTime RequestedAt { get; set; }
        // Date and time when the cancellation was processed
        public DateTime? ProcessedAt { get; set; }
        // ID of the admin or system that processed the cancellation
        public int? ProcessedBy { get; set; } // Could link to an Admin entity if available
        // The order amount at the time of cancellation request initiation.
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderAmount { get; set; }
        // The cancellation charges applied (if any).
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CancellationCharges { get; set; } = 0.00m;
        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string? Remarks { get; set; }
        //Reference Navigation Property
        public Refund? Refund { get; set; }
}
