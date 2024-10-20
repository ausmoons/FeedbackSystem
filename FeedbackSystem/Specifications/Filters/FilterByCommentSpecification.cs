using FeedbackSystem.Models.Entities;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Specifications.Filters
{
    public class FilterByCommentSpecification : IFeedbackSpecification
    {
        public FilterByCommentSpecification(string comment)
        {
            _comment = comment;
        }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            if (string.IsNullOrWhiteSpace(_comment))
            {
                return query;
            }

            return query.Where(f => f.Comment != null && f.Comment.ToLower().Contains(_comment.ToLower()));
        }

        #region Fields
        private readonly string _comment;
        #endregion
    }
}
