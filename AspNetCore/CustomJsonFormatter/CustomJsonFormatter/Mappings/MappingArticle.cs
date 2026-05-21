using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Models;
using AutoMapper;

namespace CustomJsonFormatter.Mappings;

public class MappingArticle : AutoMapper.Profile
{
    public MappingArticle()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : null));

        //CreateMap<Profile, ProfileDto>();
    }
}
