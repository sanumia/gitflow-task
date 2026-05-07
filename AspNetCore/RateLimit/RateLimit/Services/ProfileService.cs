using RateLimit.Models;
using System.Text.Json;

namespace RateLimit.Services;

public class ProfileService : IProfileService
{
    private readonly List<Profile> _profiles;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public ProfileService(IWebHostEnvironment env)
    {
        var filePath = Path.Combine(env.ContentRootPath, "Data", "profiles.json");
        var json = File.ReadAllText(filePath);
        _profiles = JsonSerializer.Deserialize<List<Profile>>(json, _jsonOptions)
                    ?? new List<Profile>();
    }
    public async Task<PagedResultModel<Profile>> GetProfilesAsync(ProfileQueryModel query)
    {
        await Task.Delay(500);

        IEnumerable<Profile> result = _profiles;

        result = ApplyFiltering(result, query);
        result = ApplySorting(result, query);
        var (items, totalCount) = ApplyPaging(result, query);

        return new PagedResultModel<Profile>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize,
            Query = query
        };
    }

    private static IEnumerable<Profile> ApplyFiltering(IEnumerable<Profile> source, ProfileQueryModel query)
    {
        if (!string.IsNullOrWhiteSpace(query.FirstName))
            source = source.Where(p => p.FirstName.Contains(query.FirstName, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.LastName))
            source = source.Where(p => p.LastName.Contains(query.LastName, StringComparison.OrdinalIgnoreCase));

        if (query.BirthdayFrom.HasValue)
            source = source.Where(p => p.Birthday >= query.BirthdayFrom.Value);

        if (query.BirthdayTo.HasValue)
            source = source.Where(p => p.Birthday <= query.BirthdayTo.Value);

        return source;
    }

    private static IEnumerable<Profile> ApplySorting(IEnumerable<Profile> source, ProfileQueryModel query)
    {
        var sortBy = query.SortBy ?? "LastName";
        var sortDirection = query.SortDirection ?? "asc";

        return sortBy.ToLower() switch
        {
            "firstname" => sortDirection == "desc"
                ? source.OrderByDescending(p => p.FirstName)
                : source.OrderBy(p => p.FirstName),
            "birthday" => sortDirection == "desc"
                ? source.OrderByDescending(p => p.Birthday)
                : source.OrderBy(p => p.Birthday),
            _ => sortDirection == "desc"
                ? source.OrderByDescending(p => p.LastName)
                : source.OrderBy(p => p.LastName)
        };
    }

    private static (List<Profile> Items, int TotalCount) ApplyPaging(IEnumerable<Profile> source, ProfileQueryModel query)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        return (items, totalCount);
    }
}
