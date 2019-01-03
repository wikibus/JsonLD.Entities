/**
# Documentation

## Ensuring serialized object is compacted

First let's import the required namespaces.
**/

using JsonLD.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

public class EnsuringCompactedJson
{

/**

It is possible to use absolute URIs as `[JsonProperty]` so that the serialized object doesn't need to use
a `@context` at all. This is especially useful for example if a class is used as nested objects and defining
context on each class in the tree would add one on each level, even if it redefines the same terms.

``` json
{
  "@context": { 
    "foaf": "http://xmlns.com/foaf/0.1/",
    "name": "foaf:name",
    "interest": "foaf:interest"
  },
  "@type": "Person",
  "name": "Tomasz Pluskiewicz",
  "interest": {
    "@context": {
      "foaf": "http://xmlns.com/foaf/0.1/",
      "name": "foaf:name"
    },
    "name": "JSON-LD"
  }
}
```   

This can be avoided by mapping properties to absolute 

**/

public class PersonWithInterest
{
    [JsonProperty("http://xmlns.com/foaf/0.1/interest")]
    public Interest Interest { get; set;}
        
    [JsonProperty("http://xmlns.com/foaf/0.1/name")]
    public string Name { get; set; }
}

public class Interest
{
    [JsonProperty("http://xmlns.com/foaf/0.1/name")]
    public string Name { get; set; }
}

private static readonly JObject Context = JObject.Parse(@"{ 
    'foaf': 'http://xmlns.com/foaf/0.1/',
    'name': 'foaf:name',
    'interest': 'foaf:interest'
  }");

[Fact]
public void ShouldForceCompactSerializedModelWhenRequested()
{
    // given
    var person = new PersonWithInterest
    {
        Name = "Tomasz Pluskiewicz",
        Interest = new Interest { Name = "JSON-LD" }
    };
    var contextProvider = new StaticContextProvider();
    contextProvider.SetContext(typeof(PersonWithInterest), Context);
    var serializer = new EntitySerializer(contextProvider);

    // when
    dynamic serialized = serializer.Serialize(person, new SerializationOptions { SerializeCompacted = true });

    // then
    Assert.Equal("Tomasz Pluskiewicz", serialized.name.ToString());
    Assert.Equal("JSON-LD", serialized.interest.name.ToString());
}

/**
It is also possible to force this behaviour on by annotating a class with the `[SerializeCompacted]`
attrbute. This could be useful in scenarios where the owner of the serialized classes doesn't control how
and when the `EntitySerializer` is called.
**/

[SerializeCompacted]
public class PersonWithInterest2
{
    [JsonProperty("http://xmlns.com/foaf/0.1/interest")]
    public Interest Interest { get; set;}
        
    [JsonProperty("http://xmlns.com/foaf/0.1/name")]
    public string Name { get; set; }
}

[Fact]
public void ShouldForceCompactSerializedModelWhenSetByAttribute()
{
    // given
    var person = new PersonWithInterest2
    {
        Name = "Tomasz Pluskiewicz",
        Interest = new Interest { Name = "JSON-LD" }
    };
    var contextProvider = new StaticContextProvider();
    contextProvider.SetContext(typeof(PersonWithInterest2), Context);
    var serializer = new EntitySerializer(contextProvider);

    // when
    dynamic serialized = serializer.Serialize(person);

    // then
    Assert.Equal("Tomasz Pluskiewicz", serialized.name.ToString());
    Assert.Equal("JSON-LD", serialized.interest.name.ToString());
}

/**
And this also works on inherited classes.
**/

public class PersonWithInterest3: PersonWithInterest2
{
}

[Fact]
public void ShouldForceCompactSerializedModelWhenSetByAttribute_Inherited()
{
    // given
    var person = new PersonWithInterest3
    {
        Name = "Tomasz Pluskiewicz",
        Interest = new Interest { Name = "JSON-LD" }
    };
    var contextProvider = new StaticContextProvider();
    contextProvider.SetContext(typeof(PersonWithInterest3), Context);
    var serializer = new EntitySerializer(contextProvider);

    // when
    dynamic serialized = serializer.Serialize(person);

    // then
    Assert.Equal("Tomasz Pluskiewicz", serialized.name.ToString());
    Assert.Equal("JSON-LD", serialized.interest.name.ToString());
}
}
