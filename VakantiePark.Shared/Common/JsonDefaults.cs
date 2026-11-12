using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VakantiePark.Shared.Common
{
    public static class JsonDefaults
    {
        public static JsonSerializerOptions Options { get; } = Configure(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        public static JsonSerializerOptions Configure(JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            if (!options.Converters.OfType<JsonStringEnumConverter>().Any())
                options.Converters.Add(new JsonStringEnumConverter());
            if (!options.Converters.OfType<UtcDateTimeJsonConverter>().Any())
                options.Converters.Add(new UtcDateTimeJsonConverter());

            return options;
        }
    }

    public sealed class UtcDateTimeJsonConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                throw new JsonException("Een datum mag niet leeg zijn.");

            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var date))
                throw new JsonException($"'{value}' is geen geldige datum.");

            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var utc = value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            };

            writer.WriteStringValue(utc.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
