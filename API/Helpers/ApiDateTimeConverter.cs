using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helpers;

public class ApiDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _encodeFormat;
    private readonly string[] _decodeFormats;

    public ApiDateTimeConverter(string encodeFormat, string[]? decodeFormats)
    {
        _encodeFormat = encodeFormat;
        _decodeFormats = decodeFormats ?? Array.Empty<string>();
    }

    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(_encodeFormat));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString()!, _decodeFormats, null);
    }
}