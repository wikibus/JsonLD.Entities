using System;
using System.Net;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;

public class IPAddressConverter : JsonLdLiteralConverter
{
    protected override object DeserializeLiteral(JsonReader reader, Type objectType, JsonSerializer serializer)
    {
        return IPAddress.Parse((string)reader.Value);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IPAddress);
    }
}