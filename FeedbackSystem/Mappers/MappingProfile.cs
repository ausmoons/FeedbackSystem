using AutoMapper;
using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;

namespace FeedbackSystem.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
