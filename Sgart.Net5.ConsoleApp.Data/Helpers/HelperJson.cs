using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sgart.Net5.ConsoleApp.Data.Helpers
{
    /// <summary>
    /// imposto le opzioni per serializzare in Json
    /// PropertyNameCaseInsensitive
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
                    PropertyNameCaseInsensitive = true
                };
                _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }
            return _jsonSerializerOptions;
        }
    }
}

