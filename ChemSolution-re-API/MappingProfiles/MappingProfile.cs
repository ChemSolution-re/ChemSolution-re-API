using AutoMapper;
using ChemSolution_re_API.DTO;
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
            CreateMap<User, AuthorizeResponse>()
                .ForMember(x => x.UserId, opts => opts.MapFrom(s => s.Id));

            CreateMap<BlogPost, BlogPostDTO>();
            CreateMap<BlogPostDTO, BlogPost>();

            CreateMap<MaterialGroup, MaterialGroupDTO>();
            CreateMap<MaterialGroupDTO, MaterialGroup>();
        }
    }
}
