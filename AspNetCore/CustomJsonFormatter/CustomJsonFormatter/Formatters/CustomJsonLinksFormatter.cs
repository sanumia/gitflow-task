using CustomJsonFormatter.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CustomJsonFormatter.Formatters;

public class CustomJsonLinksFormatter : TextOutputFormatter
{
    public CustomJsonLinksFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json+custom"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (type == null) return false;

        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var httpContext = context.HttpContext;
        var request = httpContext.Request;
        var responseObject = context.Object;

        if (responseObject == null)
        {
            await httpContext.Response.WriteAsync("null", selectedEncoding);
            return;
        }

        var wrapped = WrapWithLinks(responseObject, request);
        var json = JsonSerializer.Serialize(wrapped);
        await httpContext.Response.WriteAsync(json, selectedEncoding);
    }

    private object WrapWithLinks(object obj, HttpRequest request)
    {
        string BuildUrl(string path) => $"{request.Scheme}://{request.Host}{path}";

        if (obj is Article article)
        {
            return new ResponseJson<Article>
            {
                Data = article,
                Links = new Dictionary<string, string>
                {
                    ["self"] = BuildUrl($"/api/article/{article.Id}"),
                    ["get-author"] = BuildUrl($"/api/profile/{article.AuthorId}")
                }
            };
        }

        if (obj is Profile profile)
        {
            return new ResponseJson<Profile>
            {
                Data = profile,
                Links = new Dictionary<string, string>
                {
                    ["self"] = BuildUrl($"/api/profile/{profile.Id}")
                }
            };
        }

        if (obj is IEnumerable<Article> articles)
        {
            return articles.Select(a => WrapWithLinks(a, request));
        }

        if (obj is IEnumerable<Profile> profiles)
        {
            return profiles.Select(p => WrapWithLinks(p, request));
        }

        return obj;
    }
}
