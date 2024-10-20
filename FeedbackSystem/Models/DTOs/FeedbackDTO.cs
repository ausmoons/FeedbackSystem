using System.ComponentModel.DataAnnotations;

namespace FeedbackSystem.Models.DTOs
{
    public class FeedbackDTO
    {
        [Required(ErrorMessage = "Customer Id is required.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
