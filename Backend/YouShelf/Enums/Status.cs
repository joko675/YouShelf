using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    READING,
    READ,
    PLANNING,
    CANCELLED
}