using System.Text.Json;

namespace BlazingChat.Client.Helper
{
    public static class JsonConvertor
    {
        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize(object value, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            return JsonSerializer.Serialize(value, jsonSerializerOptions ?? JsonSerializerOptions);
        }

        public static TResult? Deserialize<TResult>(string seralizedstring, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            return JsonSerializer.Deserialize<TResult>(seralizedstring, jsonSerializerOptions ?? JsonSerializerOptions);
        }
    }
}
