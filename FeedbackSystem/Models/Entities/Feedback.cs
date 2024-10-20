using System.ComponentModel.DataAnnotations;

namespace FeedbackSystem.Models.Entities
{
    public class Feedback
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
