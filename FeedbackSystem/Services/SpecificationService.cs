using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Services.Interfaces;
using FeedbackSystem.Specifications.Filters;
using FeedbackSystem.Specifications.Interfaces;

namespace FeedbackSystem.Services
{
    public class SpecificationService : ISpecificationService
    {
        public List<IFeedbackSpecification> GetSpecifications(FeedbackFilter filter)
        {
            var specifications = new List<IFeedbackSpecification>();

            if (filter != null)
            {
                if (filter.Id.HasValue && filter.Id > 0)
                {
                    specifications.Add(new FilterByIdSpecification(filter.Id.Value));
                }

                if (filter.CustomerId.HasValue && filter.CustomerId > 0)
                {
                    specifications.Add(new FilterByCustomerIdSpecification(filter.CustomerId.Value));
                }

                if (filter.ProductId.HasValue && filter.ProductId > 0)
                {
                    specifications.Add(new FilterByProductiIdSpecification(filter.ProductId.Value));
                }

                if (filter.Rating.HasValue && filter.Rating > 0)
                {
                    specifications.Add(new FilterByRatingSpecification(filter.Rating.Value));
                }

                if (!string.IsNullOrEmpty(filter.Comment))
                {
                    specifications.Add(new FilterByCommentSpecification(filter.Comment));
                }
            }

            return specifications;
        }
    }

}
