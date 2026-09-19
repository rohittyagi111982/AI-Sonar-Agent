using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SonarDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return default;

        // Convert +0530 -> +05:30
        if (value.Length > 5 &&
            (value[^5] == '+' || value[^5] == '-'))
        {
            value = value.Insert(value.Length - 2, ":");
        }

        return DateTime.Parse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind);
    }

    public override void Write(Utf8JsonWriter writer,
        DateTime value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}