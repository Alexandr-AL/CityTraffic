using System.Net;

namespace CityTraffic.Infrastructure.GortransPermApi
{
    public class GortransPermApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string RequestUrl { get; }

        public string ResponseContent { get; }

        public GortransPermApiException() { }

        public GortransPermApiException(string message) : base(message) { }

        public GortransPermApiException(string message, Exception innerException) : base(message, innerException) { }

        public GortransPermApiException(string message,
                                     HttpStatusCode httpStatusCode,
                                     string requestUrl,
                                     string responseContent,
                                     Exception innerException = null) : base(message, innerException)
        {
            StatusCode = httpStatusCode;
            RequestUrl = requestUrl;
            ResponseContent = responseContent;
        }

        public override string ToString()
        {
            return $"""
                GortransPermException: {Message}
                StatusCode: {StatusCode} ({(int)StatusCode})
                RequsetUrl: {RequestUrl}
                ResponseContent: {ResponseContent}
                StackTrace: {StackTrace}
                InnerException: {InnerException?.ToString() ?? "None"}
                """;
        }
    }
}
