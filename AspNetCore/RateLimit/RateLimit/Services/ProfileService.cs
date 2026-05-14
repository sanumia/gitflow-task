using RateLimit.Models;
using RateLimit.Models.Enums;
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
        _profiles = JsonSerializer.Deserialize<List<Profile>>(json, _jsonOptions) ?? [];
    }

    public async Task<PagedResultModel<Profile>> GetProfilesAsync(ProfileQueryModel query)
    {
        await Task.Delay(500);

        IEnumerable<Profile> result = _profiles;

        result = ApplyFiltering(result, query);

        var sortBy = query.SortBy ?? SortFieldType.LastName;
        var sortDirection = query.SortDirection ?? SortDirectionType.Asc;
        result = ApplySorting(result, sortBy, sortDirection);

        var items = ApplyPaging(result, query.PageNumber, query.PageSize);

        return new PagedResultModel<Profile>
        {
            Items = items,
            TotalCount = result.Count(),
            Page = query.PageNumber,
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

    private static IEnumerable<Profile> ApplySorting(
        IEnumerable<Profile> source,
        SortFieldType sortBy,
        SortDirectionType sortDirection)
    {
        var keySelector = GetKeySelector(sortBy);

        return ApplyOrdering(source, keySelector, sortDirection);
    }

    private static Func<Profile, object> GetKeySelector(SortFieldType sortBy) =>
    sortBy switch
    {
        SortFieldType.FirstName => p => p.FirstName,
        SortFieldType.Birthday => p => p.Birthday,
        _ => p => p.LastName
    };

    private static IEnumerable<Profile> ApplyOrdering(
        IEnumerable<Profile> source,
        Func<Profile, object> keySelector,
        SortDirectionType direction)
    {
        return direction == SortDirectionType.Desc
            ? source.OrderByDescending(keySelector)
            : source.OrderBy(keySelector);
    }
    private static List<Profile> ApplyPaging(IEnumerable<Profile> source, int pageNumber, int pageSize)
    {
        return source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}
