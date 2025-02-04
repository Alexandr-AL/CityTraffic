using System.Net;

namespace CityTraffic.Services.GortransPerm
{
    public class GortransPermException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string RequestUrl { get; }

        public string ResponseContent { get; }

        public GortransPermException() { }

        public GortransPermException(string message) : base(message) { }

        public GortransPermException(string message, Exception innerException) : base(message, innerException) { }

        public GortransPermException(string message,
                                     HttpStatusCode httpStatusCode,
                                     string requestUrl,
                                     string responseContent,
                                     Exception innerException = null) : base(message, innerException)
        {
            StatusCode = httpStatusCode;
            RequestUrl = requestUrl;
            ResponseContent = responseContent;
        }

        public string UserMessage()
        {
            return $"""
                GortransPermException: {Message}
                StatusCode: {StatusCode} ({(int)StatusCode})
                RequsetUrl: {RequestUrl}
                """;
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
