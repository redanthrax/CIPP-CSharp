using CIPP.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CIPP.Api.Extensions;

public static class ResultsExtensions {
    public static IResult ErrorResponse<T>(string message, int statusCode = 500, List<string>? errors = null) {
        return Results.Json(
            Response<T>.ErrorResult(message, errors ?? new List<string>()),
            statusCode: statusCode
        );
    }

    public static IResult ErrorResponse<T>(string message, Exception ex, int statusCode = 500) {
        return Results.Json(
            Response<T>.ErrorResult(message, new List<string> { ex.Message }),
            statusCode: statusCode
        );
    }

    public static IResult BadRequestResponse<T>(string message, List<string>? errors = null) {
        return Results.Json(
            Response<T>.ErrorResult(message, errors ?? new List<string>()),
            statusCode: 400
        );
    }

    public static IResult NotFoundResponse<T>(string message) {
        return Results.Json(
            Response<T>.ErrorResult(message, new List<string>()),
            statusCode: 404
        );
    }
}
