/**
# Documentation

## Working with literal values

As always, here are the required namespace imports.
**/

using System;
using JsonLD.Entities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

/**
### Deserialize expanded form of literals

JSON-LD have two ways of explicitly representing typed literal. They can be typed in the `@context`, in which case the JSON property value is
a simple string token.

``` js
{
  "@context": {
    "age": {
      "@id": "http://example.com/onto#age",
      "@type": "http://www.w3.org/2001/XMLSchema#integer"  
    }
  },
  "age": "28"
}
```

The other way is to directly state the value's type by expanding the literal to JSON object. See how the `@type` is moved from the 
`@context` and a `@value` keyword is introduced.

``` js
{
  "@context": {
    "age": {
      "@id": "http://example.com/onto#age"
    }
  },
  "age": {
    "@value": "28",
    "@type": "http://www.w3.org/2001/XMLSchema#integer"
  }
}
```

The two examples above may be equivalent but the second one, which you will probably commonly encounter when working with JSON-LD, poses a
problem for default Newtonsoft.Json serializer, because JSON object is not expected for primitive type such as `int` or `bool`.
**/

[TestFixture]
public class DeserializationOfLiterals
{

/**
JsonLd.Entities will try to deserialize the `@value` instead. Note however that if the actual type and C# property types don't match a
round trip deserialization and serialization could produce different JSON-LD.
**/

public class PersonWithAge
{
    public Uri Id { get; set; }

    public long Age { get; set; }
}

[Test]
public void Can_deserialize_expanded_literal()
{
    // given
    var jsonLd = JObject.Parse(@"
{
  '@context': {
    'age': {
      '@id': 'http://example.com/onto#age'
    }
  },
  'age': {
    '@value': '28',
    '@type': 'http://www.w3.org/2001/XMLSchema#integer'
  }
}");

    // when
    IEntitySerializer serializer = new EntitySerializer(new StaticContextProvider());
    var person = serializer.Deserialize<PersonWithAge>(jsonLd);

    // then
    Assert.That(person.Age, Is.EqualTo(28));
}
}
