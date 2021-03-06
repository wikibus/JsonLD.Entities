﻿/**
# Documentation

## URI values in JSON-LD

In JSON-LD there are two ways to represent a IRI reference. The basic syntax is the expanded form,
where the property value is actually a JSON object

``` json
{
  "@context": {
    "property": "http://example.com/property"
  },
  "property": {
    "@id": "http://example.com/value"
  }
}
```

It is however possible to modify the `@context` so that the property value is simple string in JSON,
while it represents the same triples. This technique is called [type coercion][coercion].
The above example can be simplified to

``` json
{
  "@context": {
    "property": { 
      "@id": "http://example.com/property",
      "@type": "@id"
    }
  },
  "property": "http://example.com/value"
}
```

When [`@context` is removed](http://tinyurl.com/hg6wfpu) both trees above are represented as

``` json
{
  "http://example.com/property": {
    "@id": "http://example.com/value"
  }
}
```

Notice how the definition of `property` changed in the `@context`. Otherwise the value `"http://example.com/value"`
would be interpreted as simple string and [upon removing the context](http://tinyurl.com/jz7usbs) other RDF triples would be returned.

``` json
{
  "http://example.com/property": "http://example.com/value"
}
```

Let's see hot handle this in JsonLd.Entities:
**/

using System;
using JsonLD.Core;
using JsonLD.Entities;
using JsonLD.Entities.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

public class SerializingUriProperties
{
    private const string PropertyUri = "http://example.com/property";
    private const string UriValue = "http://example.com/value";

    readonly IEntitySerializer serializer = new EntitySerializer();

/**

## Serializing URI properties

### Default behaviour (without `@context`)

By default `System.Uri` values are serialized as strings. If there is no `@context`, the property must be mapped
to an absolute URL using the `[JsonProperty]` attribute
**/

private class UriPropertyMappedToAbsoluteUri
{
    [JsonProperty(PropertyUri)]
    public Uri Property { get; set; }
}

[Fact]
public void Should_serialize_URI_values_as_strings()
{
    // given       
    var serialized = this.serializer.Serialize(new UriPropertyMappedToAbsoluteUri { Property = new Uri(UriValue) });

    // when 
    // to remove @context, an empty JObject can be passed
    var noContext = JsonLdProcessor.Compact(serialized, new JObject(), new JsonLdOptions());

    // then
    Assert.Equal(UriValue, noContext[PropertyUri].ToString());
}

/**

This behaviour is by design, so that the resulting JSON object is as idiomatic as possible ou of the box.
It is up to the consumer to supply proper `@context`, which ensures that [type coercion][coercion] is applied.

### Default property name (with `@context`)

Instead of mapping a property to full predicate URI as in class `UriPropertyMappedToAbsoluteUri` above it is
possible to define the mapping in the context so that default JSON serialization rules apply and have the context
determins how values expand to RDF terms.

**/

private class UriPropertyWithContext
{
    public Uri Property { get; set; }

    public static JObject Context
    {
        get
        {
            return new JObject
            {
                "property".IsProperty(PropertyUri).Type().Id()
            };
        }
    }
}

[Fact]
public void Should_serialize_URI_values_as_strings_with_context()
{
    // given       
    var serialized = this.serializer.Serialize(new UriPropertyWithContext { Property = new Uri(UriValue) });

    // when 
    // to remove @context, an empty JObject can be passed
    var noContext = JsonLdProcessor.Compact(serialized, new JObject(), new JsonLdOptions());

    // then
    Assert.Equal(UriValue, noContext[PropertyUri]["@id"].ToString());
}

/**

### Forcing serializing expanded URI

There may be circumstances, such as with an externally supplied `@context`, that in may be necessary to
ensure that the `Uri` value is serialized in it's expanded form. For that there is the `IriRef` struct.

**/

private class UriPropertyForcedToExpand
{
    public IriRef Property { get; set; }

    public static JObject Context
    {
        get
        {
            return new JObject
            {
                "property".IsProperty(PropertyUri)
            };
        }
    }
}

[Fact]
public void Should_serialize_URI_values_as_expanded_object()
{
    // given       
    var serialized = this.serializer.Serialize(new UriPropertyForcedToExpand { Property = new IriRef(UriValue) });

    // when 
    // to remove @context, an empty JObject can be passed
    var noContext = JsonLdProcessor.Compact(serialized, new JObject(), new JsonLdOptions());

    // then
    Assert.Equal(UriValue, noContext[PropertyUri]["@id"].ToString());
}
}

/**
[coercion]: https://www.w3.org/TR/json-ld/#type-coercion
**/