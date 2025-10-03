namespace CIPP.Frontend.WASM.Modules.Authentication.Exceptions;

public class UnauthorizedApiException : Exception {
    public string Endpoint { get; }
    public int StatusCode { get; }

    public UnauthorizedApiException(string endpoint, int statusCode)
        : base($"Unauthorized access to endpoint '{endpoint}'. Status code: {statusCode}") {
        Endpoint = endpoint;
        StatusCode = statusCode;
    }

    public UnauthorizedApiException(string endpoint, int statusCode, string message)
        : base(message) {
        Endpoint = endpoint;
        StatusCode = statusCode;
    }

    public UnauthorizedApiException(string endpoint, int statusCode, string message, Exception innerException)
        : base(message, innerException) {
        Endpoint = endpoint;
        StatusCode = statusCode;
    }
}