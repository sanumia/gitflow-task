using CustomJsonFormatter.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CustomJsonFormatter.Formatters;

public class CustomJsonLinksFormatter : TextOutputFormatter
{
    private const string MediaTypeForCustomParsing = "application/json+custom";
    private const string SelfLinkKey = "self";
    private const string GetAuthorLinkKey = "get-author";

    public CustomJsonLinksFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeForCustomParsing));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        return type != null;
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

        return obj switch
        {
            Article article => new ResponseJson<Article>
            {
                Data = article,
                Links = new Dictionary<string, string>
                {
                    [SelfLinkKey] = BuildUrl($"/api/article/{article.Id}"),
                    [GetAuthorLinkKey] = BuildUrl($"/api/profile/{article.AuthorId}")
                }
            },
            Profile profile => new ResponseJson<Profile>
            {
                Data = profile,
                Links = new Dictionary<string, string>
                {
                    [SelfLinkKey] = BuildUrl($"/api/profile/{profile.Id}")
                }
            },
            IEnumerable<Article> articles => articles.Select(a => WrapWithLinks(a, request)),
            IEnumerable<Profile> profiles => profiles.Select(p => WrapWithLinks(p, request)),
            _ => throw new InvalidOperationException($"Unexpected object type")
        };
    }
}
