/**
# Documentation

## Serializing typed entities

In JSON-LD (and RDF in general), types of an object are declared with a property like any other, called rdf:type. In JSON-LD though it is
represented as a special `@type` property. Below examples show how to defines types and have them serialized to JSON.

First let's import the required namespaces.
 **/

using System;
using System.Collections.Generic;
using JsonLD.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

[TestFixture]
public class SerializingTypedEntities
{

/**
### Declaring types explicitly

Easiest way to define an instance's `@type`s is to declare a `Types` or `Type` property. It can also be private or protected, but then 
`[JsonProperty]` attribue must be used so that it's serialized. Similarily to the `Id` property, the JSON property will be prefixed by the 
`@` character by convention. 
    
The property can return any type or collection thereof, provided that the serialized value is a string. Thus any custom converter could also
be used.

It is also be possible to use any other property and give it a proper name by setting the attribute like `[JsonProperty("@type")]`
**/

public class TypesAsSingleUri
{
    public Uri Type
    {
        get { return new Uri("http://schema.org/Person"); }
    }
}

public class TypesAsStringCollection
{
    public IEnumerable<string> Types
    {
        get { yield return "http://schema.org/Person"; }
    }
}

public class TypesAsPrivatePropertyWithCustomName
{
    [JsonProperty("@type")]
    private IEnumerable<Uri> Classes
    {
        get { yield return new Uri("http://schema.org/Person"); }
    }
}

[TestCase(typeof(TypesAsSingleUri), "{ '@type': 'http://schema.org/Person' }")]
[TestCase(typeof(TypesAsStringCollection), "{ '@type': [ 'http://schema.org/Person' ] }")]
[TestCase(typeof(TypesAsPrivatePropertyWithCustomName), "{ '@type': [ 'http://schema.org/Person' ] }")]
public void SerializesTypesPropertyAsAtTypes(Type type, string expectedJson)
{
    // given
    var expected = JObject.Parse(expectedJson);
    var entity = Activator.CreateInstance(type);
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.That(JToken.DeepEquals(json, expected), "Actual object was {0}",  json);
}
}