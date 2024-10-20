using Bogus;
using FeedbackSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var fakeFeedbacks = GenerateFakeFeedbackData();

            modelBuilder.Entity<Feedback>().HasData(fakeFeedbacks);
        }

        private List<Feedback> GenerateFakeFeedbackData()
        {
            var feedbackFaker = new Faker<Feedback>()
                .RuleFor(f => f.Rating, f => f.Random.Int(1, 5))
                .RuleFor(f => f.Comment, f => f.Lorem.Sentence())
                .RuleFor(f => f.CreatedAt, f => f.Date.Past(1));

            var fakeFeedbacks = feedbackFaker.Generate(10);

            for (int i = 0; i < fakeFeedbacks.Count(); i++)
            {
                fakeFeedbacks[i].Id = i + 1;
                fakeFeedbacks[i].CustomerId = i + 1;
                fakeFeedbacks[i].ProductId = i + 1;
            }

            return fakeFeedbacks;
        }
    }
}
