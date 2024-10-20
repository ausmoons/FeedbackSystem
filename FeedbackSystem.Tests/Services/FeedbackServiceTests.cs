using FeedbackSystem.Data;
using FeedbackSystem.Services;
using FeedbackSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;
using System;
using FeedbackSystem.Specifications.Filters;

namespace FeedbackSystem.Tests.Services
{
    public class FeedbackServiceTests : IDisposable
    {
        public FeedbackServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(_dbContextOptions);
            _specificationServiceMock = new Mock<ISpecificationService>();
            _loggerMock = new Mock<ILogger<FeedbackService>>();
            _feedbackService = new FeedbackService(_dbContext, _specificationServiceMock.Object, _loggerMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _dbContext.Feedbacks.AddRange(
                new Feedback { Id = 1, CustomerId = 1, ProductId = 1, Rating = 5, Comment = "Great product!" },
                new Feedback { Id = 2, CustomerId = 2, ProductId = 2, Rating = 4, Comment = "Good product!" },
                new Feedback { Id = 3, CustomerId = 3, ProductId = 1, Rating = 3, Comment = "Average product!" }
            );
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetFeedbacks_ShouldReturnPaginatedFeedbacks()
        {
            // Arrange
            var queryOptions = new QueryOptions { Page = 1, PageSize = 2 };

            // Act
            var (totalRecords, feedbackList) = await _feedbackService.GetFeedbacks(queryOptions);

            // Assert
            Assert.Equal(3, totalRecords);
            Assert.Equal(2, feedbackList.Count);
        }

        [Fact]
        public async Task GetFilteredFeedbacks_ShouldReturnFilteredFeedbacks()
        {
            // Arrange
            var filter = new FeedbackFilter { Rating = 5 };

            var specifications = new List<IFeedbackSpecification>
            {
                new FilterByRatingSpecification(filter.Rating.Value)
            };

            _specificationServiceMock.Setup(s => s.GetSpecifications(filter)).Returns(specifications);

            // Act
            var (totalRecords, feedbackList) = await _feedbackService.GetFilteredFeedbacks(filter);

            // Assert
            Assert.Equal(1, totalRecords);
            Assert.Single(feedbackList);
            Assert.Equal(5, feedbackList.First().Rating);
        }

        [Fact]
        public async Task GetFeedbacks_ShouldAdjustPaginationAndReturnResults()
        {
            // Arrange
            var queryOptions = new QueryOptions { Page = -1, PageSize = 0 };

            // Act
            var (totalRecords, feedbackList) = await _feedbackService.GetFeedbacks(queryOptions);

            // Assert
            Assert.Equal(3, totalRecords);
            Assert.Equal(3, feedbackList.Count());
        }

        [Fact]
        public async Task GetFilteredFeedbacks_ShouldLogErrorWhenExceptionThrown()
        {
            // Arrange
            var filter = new FeedbackFilter();
            _specificationServiceMock.Setup(s => s.GetSpecifications(filter)).Throws(new Exception("Test Exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _feedbackService.GetFilteredFeedbacks(filter));

            Assert.Contains("An error occurred while fetching feedbacks. Test Exception.", exception.Message);

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while fetching feedbacks")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }

        #region Fields
        private readonly Mock<ISpecificationService> _specificationServiceMock;
        private readonly Mock<ILogger<FeedbackService>> _loggerMock;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly FeedbackService _feedbackService;
        #endregion
    }
}
