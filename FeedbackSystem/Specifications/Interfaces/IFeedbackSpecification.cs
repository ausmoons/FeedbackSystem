using FeedbackSystem.Models.Entities;

namespace FeedbackSystem.Specifications.Interfaces
{
    public interface IFeedbackSpecification
    {
        IQueryable<Feedback> Apply(IQueryable<Feedback> query);
    }
}
