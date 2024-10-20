using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Specifications.Filters
{
    public class FilterByCustomerIdSpecification : IFeedbackSpecification
    {
        public FilterByCustomerIdSpecification(int customerId)
        {
            _customerId = customerId;
        }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            return query.Where(f => f.CustomerId == _customerId);
        }

        #region Fields
        private readonly int _customerId;
        #endregion
    }
}
