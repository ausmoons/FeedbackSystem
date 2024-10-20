using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Specifications.Filters
{
    public class FilterByProductiIdSpecification : IFeedbackSpecification
    {
        public FilterByProductiIdSpecification(int? productint)
        {
            _productint = productint;
        }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            if (_productint.HasValue)
            {
                query = query.Where(f => f.ProductId == _productint.Value);
            }
            return query;
        }

        #region Fields
        private readonly int? _productint;
        #endregion
    }
}
