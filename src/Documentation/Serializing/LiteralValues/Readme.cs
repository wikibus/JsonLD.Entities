using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using NodaTime.Serialization.JsonNet;
using NUnit.Framework;

/**
# Documentation

Related topic: [deserializing literals][deserialize-literals]

## Serializing literal values

In JSON-LD a literal value (ie. not a URI) are represented as JS objects, which
contains the value as string and that value's type URI. For example a date
would be represented as:

``` json
{
  "@context": "http://example.com/vocab/",
  "arrivalDate": {
    "@value": "2017-01-14T14:50Z",
    "@type": "http://www.w3.org/2001/XMLSchema#dateTime"
  }
}
```

### Serializing framework types

JsonLd.Entities serializer will choose an appropriate [XSD data type][xsd] for
matching .NET Framework primitive types and will output a JSON-LD object like the one
shown above.

**/

public class SerializingFrameworkTypes
{
    private class TrainSchedule
    {
        public System.DateTime ArrivalDate => new System.DateTime(2017, 1, 14, 14, 50, 0);
    }

    [Test]
    public void Serializes_builtin_types_as_expanded_literal()
    {
        // given 
        var serializer = new JsonLD.Entities.EntitySerializer();

        // when
        dynamic schedule = serializer.Serialize(new TrainSchedule());

        // then
        Assert.That((string)schedule.arrivalDate["@value"], Is.EqualTo("2017-01-14T14:50:00"));
        Assert.That((string)schedule.arrivalDate["@type"], Is.EqualTo("http://www.w3.org/2001/XMLSchema#dateTime"));
    }
}

/**

The above works for all .NET numeric types, `DateTime`, `DateTimeOffset` and `TimeSpan`.

The exceptions are `double` and `string` which all have their implicit typing in JSON and
so they are serialized accoring to their default JavaScript rules while retaining the
correct RDF typing (`xsd:double`, `xsd:boolean` and `xsd:string` accordingly). See the
[JSON-LD spec](https://www.w3.org/TR/json-ld/#conversion-of-native-data-types).

Note that integers are excempt from this rule, because as a generic term for all integral
numbers, such datatype does not exist in .NET.

### Serializing other types as literal

It may be required to serialize instances of other classes as JSON-LD literals.
Sticking to dates, one may prefer to use the `Instant` type from Jon Skeet's [NodaTime library][noda].

To serialize any arbitrary type as an expanded literal one has to derive from 
`JsonLdLiteralConverter` and set it as the coverter for [property][prop-conv] or [class][class-conv].

The converter class has one abstract and a number of virtual methods, which control
the serialization output. In this example we use NodaTime's converter to write the `Instant` to
JSON and set the `@type` to `xsd:dateTime`. Do have a look at this [deserialization page][deserialize-literals]
for more examples.

**/

public class SerializingCustomTypesAsLiterals
{
    public class JsonLdNodaInstantConverter : JsonLdLiteralConverter
    {
        protected override void WriteJsonLdValue(JsonWriter writer, object value, JsonSerializer serializer)
        {
            NodaConverters.InstantConverter.WriteJson(writer, value, serializer);
        }

        protected override string GetJsonLdType(object value)
        {
            return Vocab.Xsd.dateTime;
        }
    }

    public class NodaTimeTrainSchedule
    {
        [JsonConverter(typeof(JsonLdNodaInstantConverter))]
        public NodaTime.Instant ArrivalDate => NodaTime.Instant.FromUtc(2017, 1, 14, 14, 50);
    }

    [Test]
    public void Serializes_NodaTime_as_expanded_literal()
    {
        // given 
        var serializer = new JsonLD.Entities.EntitySerializer();

        // when
        dynamic schedule = serializer.Serialize(new NodaTimeTrainSchedule());

        // then
        Assert.That((string)schedule.arrivalDate["@value"], Is.EqualTo("2017-01-14T14:50:00Z"));
        Assert.That((string)schedule.arrivalDate["@type"], Is.EqualTo("http://www.w3.org/2001/XMLSchema#dateTime"));
    }
}

/**
 
[deserialize-literals]: /wikibus/JsonLD.Entities/tree/master/src/Documentation/Deserializing/LiteralValues
[xsd]: https://www.w3.org/TR/xmlschema-2/#built-in-datatypes
[noda]: https://www.nuget.org/packages/NodaTime
[class-conv]: http://www.newtonsoft.com/json/help/html/JsonConverterAttributeClass.htm
[prop-conv]: http://www.newtonsoft.com/json/help/html/JsonConverterAttributeProperty.htm

**/