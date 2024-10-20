using AutoMapper;
using FeedbackSystem.Controllers;
using FeedbackSystem.Data;
using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;
using FeedbackSystem.Services;
using FeedbackSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FeedbackSystem.Tests.Controllers
{
    public class FeedbackControllerTests : IDisposable
    {
        public FeedbackControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            _feedbackServiceMock = new Mock<IFeedbackService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<FeedbackController>>();

            _controller = new FeedbackController(_dbContext, _feedbackServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetFeedbacks_ShouldReturnPaginatedFeedbacks()
        {
            // Arrange
            var queryOptions = new QueryOptions { Page = 1, PageSize = 10 };
            var feedbackList = new List<Feedback> { new Feedback { Id = 1, Comment = "Great product" } };
            _feedbackServiceMock.Setup(s => s.GetFeedbacks(queryOptions)).ReturnsAsync((1, feedbackList));

            // Act
            var result = await _controller.GetFeedbacks(queryOptions);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task FilterFeedbacks_ShouldReturnFilteredFeedbacks()
        {
            // Arrange
            var filter = new FeedbackFilter { Rating = 5 };
            var feedbackList = new List<Feedback> { new Feedback { Id = 1, Comment = "Excellent product", Rating = 5 } };
            _feedbackServiceMock.Setup(s => s.GetFilteredFeedbacks(filter)).ReturnsAsync((1, feedbackList));

            // Act
            var result = await _controller.FilterFeedbacks(filter);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task PostFeedback_ShouldCreateFeedback()
        {
            // Arrange
            var feedbackDTO = new FeedbackDTO { Comment = "Good product", CustomerId = 1, ProductId = 1, Rating = 4 };
            var feedback = new Feedback { Id = 1, Comment = "Good product", CustomerId = 1, ProductId = 1, Rating = 4 };

            _mapperMock.Setup(m => m.Map<Feedback>(feedbackDTO)).Returns(feedback);

            // Act
            var result = await _controller.PostFeedback(feedbackDTO);
            var createdAtActionResult = result.Result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.IsType<CreatedAtActionResult>(createdAtActionResult);

            var createdFeedback = createdAtActionResult.Value as Feedback;
            Assert.NotNull(createdFeedback);
            Assert.Equal(feedbackDTO.Comment, createdFeedback.Comment);
        }

        [Fact]
        public async Task PutFeedback_ShouldUpdateFeedback()
        {
            // Arrange
            var id = 1;
            var feedbackDTO = new FeedbackDTO { Comment = "Updated comment", CustomerId = 1, ProductId = 1, Rating = 4 };

            var feedback = new Feedback { Id = 1, Comment = "Good product", CustomerId = 1, ProductId = 1, Rating = 4 };
            _dbContext.Feedbacks.Add(feedback);
            await _dbContext.SaveChangesAsync();

            var existingFeedback = await _dbContext.Feedbacks.FindAsync(id);

            _mapperMock.Setup(m => m.Map(feedbackDTO, existingFeedback)).Callback(() =>
            {
                existingFeedback.Comment = feedbackDTO.Comment;
                existingFeedback.Rating = feedbackDTO.Rating;
                existingFeedback.ProductId = feedbackDTO.ProductId;
                existingFeedback.CustomerId = feedbackDTO.CustomerId;
            });

            // Act
            var result = await _controller.PutFeedback(id, feedbackDTO);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<OkObjectResult>(okResult);

            var updatedFeedback = await _dbContext.Feedbacks.FindAsync(id);
            Assert.Equal("Updated comment", updatedFeedback.Comment);
            Assert.Equal(4, updatedFeedback.Rating);
            Assert.Equal(1, updatedFeedback.ProductId);
            Assert.Equal(1, updatedFeedback.CustomerId);
        }

        [Fact]
        public async Task DeleteFeedback_ShouldDeleteFeedback()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);

            var feedback = new Feedback { Id = 1, Comment = "Good product", CustomerId = 1, ProductId = 1, Rating = 4 };
            dbContext.Feedbacks.Add(feedback);
            await dbContext.SaveChangesAsync();

            var feedbackService = new FeedbackService(dbContext, Mock.Of<ISpecificationService>(), Mock.Of<ILogger<FeedbackService>>());
            var controller = new FeedbackController(dbContext, feedbackService, Mock.Of<IMapper>(), Mock.Of<ILogger<FeedbackController>>());

            // Act
            var result = await controller.DeleteFeedback(feedback.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);

            var deletedFeedback = await dbContext.Feedbacks.FindAsync(feedback.Id);
            Assert.Null(deletedFeedback);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); 
            _dbContext.Dispose();
        }

        #region Fields
        private readonly Mock<IFeedbackService> _feedbackServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<FeedbackController>> _loggerMock;
        private readonly AppDbContext _dbContext;
        private readonly FeedbackController _controller;
        #endregion
    }
}
