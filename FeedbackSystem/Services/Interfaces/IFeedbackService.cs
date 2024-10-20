using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;

namespace FeedbackSystem.Services.Interfaces
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Retrieves a filtered list of feedback entries based on the provided filter criteria.
        /// </summary>
        /// <param name="filter">An object containing filter criteria such as Rating, ProductId, CustomerId, etc.</param>
        /// <returns>A tuple containing the total number of records that match the filter and a list of feedback entries.</returns>
        Task<(int totalRecords, List<Feedback> feedbackList)> GetFilteredFeedbacks(FeedbackFilter filter);

        /// <summary>
        /// Retrieves a list of feedback entries based on the provided filter criteria.
        /// </summary>
        /// <param name="queryOptions">An object containingpagination inforamation.</param>
        /// <returns>A tuple containing the total number of records that are being returned and a list of feedback entries.</returns>
        Task<(int totalRecords, List<Feedback> feedbackList)> GetFeedbacks(QueryOptions queryOptions);

    }
}
