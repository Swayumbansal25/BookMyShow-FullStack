using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Payments
{
    public class ProcessPaymentDto
    {
        [Required]
        public long BookingId { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public string TransactionId { get; set; } = string.Empty;
    }
}