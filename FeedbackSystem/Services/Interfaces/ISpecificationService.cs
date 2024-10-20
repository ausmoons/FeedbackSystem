using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Services.Interfaces
{
    public interface ISpecificationService
    {
        List<IFeedbackSpecification> GetSpecifications(FeedbackFilter filter);
    }
}
