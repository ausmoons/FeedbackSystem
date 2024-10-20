using FeedbackSystem.Data;
using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;
using FeedbackSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Services
{
    public class FeedbackService : IFeedbackService
    {
        public FeedbackService(AppDbContext context,
            ISpecificationService specificationService,
            ILogger<FeedbackService> logger)
        {
            _context = context;
            _specificationService = specificationService;
            _logger = logger;
        }

        public async Task<(int totalRecords, List<Feedback> feedbackList)> GetFilteredFeedbacks(FeedbackFilter filter)
        {
            try
            {
                ValidatePagination(filter);

                var query = _context.Feedbacks.AsQueryable();

                if (filter != null)
                {
                    var specifications = _specificationService.GetSpecifications(filter);
                    foreach (var spec in specifications)
                    {
                        query = spec.Apply(query);
                    }
                }

                var totalRecords = await query.CountAsync();

                var feedbackList = await ApplyPaginationAsync(query, filter);

                return (totalRecords, feedbackList);
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while fetching feedbacks. {ex.Message}. {ex.StackTrace}";
                _logger.LogError(ex, message);

                throw new Exception(message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of feedback entries based on the provided query options (pagination parameters).
        /// </summary>
        /// <param name="queryOptions">The pagination options including the page number and page size.</param>
        /// <returns>
        /// A tuple containing the total number of feedback records (before pagination) and a list of feedback entries
        /// limited by the specified pagination options.
        /// </returns>
        public async Task<(int totalRecords, List<Feedback> feedbackList)> GetFeedbacks(QueryOptions queryOptions)
        {
            try
            {
                ValidatePagination(queryOptions);

                var query = _context.Feedbacks.AsQueryable();

                var totalRecords = await query.CountAsync();

                var feedbackList = await ApplyPaginationAsync(query, queryOptions);

                return (totalRecords, feedbackList);
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while fetching feedbacks. {ex.Message}. {ex.StackTrace}";
                _logger.LogError(ex, message);

                throw new Exception(message);
            }
        }

        /// <summary>
        /// Validates and adjusts the pagination parameters in the provided query options.
        /// Ensures that the page number is greater than 0 and the page size is within valid limits.
        /// </summary>
        /// <param name="queryOptions">
        /// The pagination options containing the page number and page size to be validated.
        /// If the values are invalid, they are adjusted to default values.
        /// </param>
        private void ValidatePagination(QueryOptions queryOptions)
        {
            if (queryOptions.Page <= 0)
            {
                queryOptions.Page = 1;
            }

            if (queryOptions.PageSize <= 0)
            {
                queryOptions.PageSize = 1000;
            }
        }

        /// <summary>
        /// Applies pagination to the provided query based on the specified query options.
        /// Skips a number of records based on the page number and limits the result set based on the page size.
        /// </summary>
        /// <param name="query">The query to which pagination should be applied.</param>
        /// <param name="queryOptions">
        /// The pagination options containing the page number and page size.
        /// The method uses these options to skip records and limit the number of results.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a list of feedback entries after applying pagination.
        /// </returns>
        private async Task<List<Feedback>> ApplyPaginationAsync(IQueryable<Feedback> query, QueryOptions queryOptions)
        {
            return await query
                .Skip((queryOptions.Page - 1) * queryOptions.PageSize)
                .Take(queryOptions.PageSize)
                .ToListAsync();
        }

        #region Fields
        private readonly AppDbContext _context;
        private readonly ISpecificationService _specificationService;
        private readonly ILogger<FeedbackService> _logger;
        #endregion
    }
}

