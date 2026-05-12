using RateLimit.Models.Enums;

namespace RateLimit.Models;

public class ProfileQueryModel
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthdayFrom { get; set; }
    public DateTime? BirthdayTo { get; set; }

    public SortFieldType? SortBy { get; set; } = SortFieldType.LastName;
    public SortDirectionType? SortDirection { get; set; } = SortDirectionType.Asc;

    public int PageNumber { get; set; } = DefaultPageNumber;
    public int PageSize { get; set; } = DefaultPageSize;
}
