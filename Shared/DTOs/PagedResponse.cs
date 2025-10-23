namespace CIPP.Shared.DTOs;
public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SkipToken { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasNextPage => !string.IsNullOrEmpty(SkipToken) || PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
