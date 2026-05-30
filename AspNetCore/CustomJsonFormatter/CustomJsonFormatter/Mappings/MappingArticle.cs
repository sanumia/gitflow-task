using CustomJsonFormatter.DTOs;
using CustomJsonFormatter.Models;

namespace CustomJsonFormatter.Mappings;

public class MappingArticle : AutoMapper.Profile
{
    public MappingArticle()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(
                dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author != null 
                    ? src.Author.Name 
                    : null));
    }
}
