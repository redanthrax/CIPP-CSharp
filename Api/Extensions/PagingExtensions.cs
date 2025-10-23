using CIPP.Shared.DTOs;
using Microsoft.AspNetCore.Http;

namespace CIPP.Api.Extensions;

public static class PagingExtensions {
    public static PagingParameters GetPagingParameters(this HttpContext context) {
        var query = context.Request.Query;
        
        var pagingParams = new PagingParameters();
        
        if (query.TryGetValue("pageNumber", out var pageNumberValue) && 
            int.TryParse(pageNumberValue, out var pageNumber)) {
            pagingParams.PageNumber = pageNumber;
        }
        
        if (query.TryGetValue("pageSize", out var pageSizeValue) && 
            int.TryParse(pageSizeValue, out var pageSize)) {
            pagingParams.PageSize = pageSize;
        }
        
        if (query.TryGetValue("skipToken", out var skipTokenValue)) {
            pagingParams.SkipToken = skipTokenValue;
        }
        
        return pagingParams;
    }

    public static PagedResponse<T> ToPagedResponse<T>(
        this IEnumerable<T> items, 
        PagingParameters pagingParams, 
        int? totalCount = null,
        string? nextSkipToken = null) {
        
        var itemsList = items.ToList();
        
        return new PagedResponse<T> {
            Items = itemsList,
            TotalCount = totalCount ?? itemsList.Count,
            PageNumber = pagingParams.PageNumber,
            PageSize = pagingParams.PageSize,
            SkipToken = nextSkipToken
        };
    }
}
