using AutoMapper;
using ChemSolution_re_API.DTO.Request;
using ChemSolution_re_API.DTO.Response;
using ChemSolution_re_API.Entities;
using ChemSolution_re_API.Services.JWT.Models;

namespace ChemSolution_re_API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<User, JwtUser>();
            CreateMap<User, UserResponse>();
            CreateMap<User, AuthorizeResponse>()
                .ForMember(x => x.UserId, opts => opts.MapFrom(s => s.Id));

            CreateMap<ElementValence, ElementValenceResponse>();
            CreateMap<ValenceRequest, ElementValence>();

            CreateMap<BlogPost, BlogPostResponse>();
            CreateMap<CreateBlogPost, BlogPost>();

            CreateMap<Material, MaterialResponse>();
            CreateMap<CreateMaterial, Material>();

            CreateMap<Achievement, AchievementResponse>();
            CreateMap<CreateAchievement, Achievement>();

            CreateMap<Request, RequestResponse>();
            CreateMap<CreateRequest, Request>();

            CreateMap<Element, ElementResponse>();
            CreateMap<CreateElement, Element>();
            CreateMap<UpdateElement, Element>();
        }
    }
}
