namespace CIPP.Shared.DTOs;

public class PagingParameters {
    private int _pageNumber = 1;
    private int _pageSize = 50;
    private const int MaxPageSize = 500;

    public int PageNumber {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 50 : value);
    }

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
    public string? SkipToken { get; set; }
}
