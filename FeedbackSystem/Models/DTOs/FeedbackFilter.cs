namespace FeedbackSystem.Models.DTOs
{
    public class FeedbackFilter : QueryOptions
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
    }
}
