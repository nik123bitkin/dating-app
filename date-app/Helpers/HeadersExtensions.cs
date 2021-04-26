using Microsoft.AspNetCore.Http;

// using Newtonsoft.Json;
// using Newtonsoft.Json.Serialization;
namespace date_app.Helpers
{
    public static class HeadersExtensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
