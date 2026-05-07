namespace RateLimit.Models;

public class ProfileQueryModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthdayFrom { get; set; }
    public DateTime? BirthdayTo { get; set; }

    public string SortBy { get; set; } = "LastName";
    public string SortDirection { get; set; } = "asc";

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
