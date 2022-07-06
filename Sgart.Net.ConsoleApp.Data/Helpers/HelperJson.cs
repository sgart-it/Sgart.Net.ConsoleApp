using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sgart.Net.ConsoleApp.Data.Helpers
{
    /// <summary>
    /// imposto le opzioni per serializzare in Json: PropertyNameCaseInsensitive
    /// esempio d'uso:
    /// JsonSerializer.Serialize(v, Helpers.HelperJson.GetJsonOtions());,
    /// JsonSerializer.Deserialize<TodoData>(v, Helpers.HelperJson.GetJsonOtions())
    /// </summary>
    public static class HelperJson
    {
        private static JsonSerializerOptions _jsonSerializerOptions = null;
        public static JsonSerializerOptions GetJsonOtions()
        {
            if (_jsonSerializerOptions == null)
            {
                _jsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                   
                };
                _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                _jsonSerializerOptions.Converters.Add(new DateTimeConverter());
            }

            return _jsonSerializerOptions;
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}

