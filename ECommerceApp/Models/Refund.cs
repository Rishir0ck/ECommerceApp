using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models;

public class Refund
{
    public int Id { get; set; }
        // Foreign key to Cancellation
        [Required(ErrorMessage = "Cancellation ID is required.")]
        public int CancellationId { get; set; }
        [ForeignKey("CancellationId")]
        public Cancellation? Cancellation { get; set; }
        // Foreign key to Payment
        [Required(ErrorMessage = "Payment ID is required.")]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }
        // Amount to be refunded
        [Range(0.01, 100000.00, ErrorMessage = "Refund amount must be between $0.01 and $100,000.00.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        // Status of the Refund
        [Required]
        public RefundStatus Status { get; set; }
        [Required]
        public string? RefundMethod { get; set; }
        [StringLength(500, ErrorMessage = "Refund Reason cannot exceed 500 characters.")]
        public string? RefundReason { get; set; }
        // Transaction ID from the payment gateway
        [StringLength(100, ErrorMessage = "Transaction ID cannot exceed 100 characters.")]
        public string? TransactionId { get; set; }
        // Date and time when the refund was initiated
        [Required]
        public DateTime InitiatedAt { get; set; }
        // Date and time when the refund was completed
        public DateTime? CompletedAt { get; set; }
        //Track who processed (approved) the refund.
        public int? ProcessedBy { get; set; }
}
