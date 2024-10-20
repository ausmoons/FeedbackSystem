using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Filters;

namespace FeedbackSystem.Tests.Specifications
{
    public class FilterByCommentSpecificationTests
    {
        public FilterByCommentSpecificationTests()
        {
            _feedbacks = new List<Feedback>
            {
                new Feedback { Comment = "Great product!" },
                new Feedback { Comment = "Good value." },
                new Feedback { Comment = "great value!" },
                new Feedback { Comment = "Average experience." },
                new Feedback { Comment = null }
            }.AsQueryable();
        }

        [Fact]
        public void Apply_WithCaseInsensitiveMatch_ShouldReturnFilteredQuery()
        {
            // Arrange
            var spec = new FilterByCommentSpecification("great");

            // Act
            var result = spec.Apply(_feedbacks);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.Comment == "Great product!");
            Assert.Contains(result, f => f.Comment == "great value!");
        }

        [Fact]
        public void Apply_WithNonMatchingComment_ShouldReturnEmpty()
        {
            // Arrange
            var spec = new FilterByCommentSpecification("bad");

            // Act
            var result = spec.Apply(_feedbacks);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Apply_WithNullComment_ShouldReturnOriginalQuery()
        {
            // Arrange
            var spec = new FilterByCommentSpecification(null);

            // Act
            var result = spec.Apply(_feedbacks);

            // Assert
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Apply_WithEmptyComment_ShouldReturnOriginalQuery()
        {
            // Arrange
            var spec = new FilterByCommentSpecification("");

            // Act
            var result = spec.Apply(_feedbacks);

            // Assert
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Apply_WithWhitespaceComment_ShouldReturnOriginalQuery()
        {
            // Arrange
            var spec = new FilterByCommentSpecification("   ");

            // Act
            var result = spec.Apply(_feedbacks);

            // Assert
            Assert.Equal(5, result.Count());
        }

        #region Fields
        private readonly IQueryable<Feedback> _feedbacks;
        #endregion
    }
}
