using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Specifications.Filters
{
    public class FilterByIdSpecification : IFeedbackSpecification
    {
        public FilterByIdSpecification(int? id)
        {
            _id = id;
        }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            if (_id.HasValue)
            {
                query = query.Where(f => f.Id == _id.Value);
            }
            return query;
        }

        #region Fields
        private readonly int? _id;
        #endregion
    }
}
