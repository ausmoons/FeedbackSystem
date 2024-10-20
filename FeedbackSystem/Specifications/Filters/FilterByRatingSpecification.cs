using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Specifications.Filters
{
    public class FilterByRatingSpecification : IFeedbackSpecification
    {
        public FilterByRatingSpecification(int? rating)
        {
            _rating = rating;
        }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            if (_rating.HasValue)
            {
                query = query.Where(f => f.Rating == _rating.Value);
            }
            return query;
        }

        #region Fields
        private readonly int? _rating;
        #endregion
    }
}
