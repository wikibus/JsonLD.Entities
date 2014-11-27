[![Build status](https://ci.appveyor.com/api/projects/status/u4riv8ftspthkvgh/branch/master?svg=true)](https://ci.appveyor.com/project/tpluscode78631/jsonld-entities/branch/master)
 
# JsonLD.Entities
 
JsonLD.Entities is a small library for serializing and deserializing POCOs in JSON-LD.
 
## What is JSON-LD?
 
If you are not familiar with JSON-LD you could first go to it's [main website][jsonld].
 
JSON-LD is the newest [RDF][rdf] serialization format. It means that serialized 
data has structure and unambiguous meaning. For example if there is a `name` property, it can be identified becasue the `name` relation 
itself can be identified by the use of URI (and usually URLs).

In general JSON-LD has a familiar json structure, which makes it best suited as the entrypoint to the Semantic Web at large. It works by 
simply extending the typical json document structure. In reality it gets a _little_ bit more complex, because json has a tree structure,
but RDF represents graphs. And it is possible to represent a graph as multiple equivalent trees.

To learn more about JSON-LD you should visit its [formal specification][jsonld-spec] and the [playground][playground], where you can 
experiment.

## Using JsonLD.Entities

This entire page is generated for actual C# code. You can view it in the [Readme.cs][readme] file and run the tests
when you open the solution. Each example below and on the wiki is a compiled and runnable unit test.

First let's import the required namespaces.
 

``` c#
using System;
using JsonLD.Entities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
```

### Deserialization
 

``` c#
public class Deserialization
{
```

#### Deserializing the model as-is

The easiest operation possible is to deserialize a JSON-LD object without any changes. The example models will be deserialized to 
instances of a Person class.
 

``` c#
public class Person
{
    public Uri Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }
}
```

Serialization and deserialization is done by instances of IEntitySerializer. It's default implementation requires you to pass a 
IContextProvider, which provides [@context][jsonld-context] objects for serialized types. Because, we don't want to use a context in the
first test, the IContextProvider object won't be set up in any way.

Note how the JSON-LD `@id` is by convention deserialized to the `Person#Id` property.

``` c#
[Test]
public void Can_deserialize_with_existing_context()
{
    // given
    var json = JObject.Parse(@"
    {
        '@context': {
           'foaf': 'http://xmlns.com/foaf/0.1/',
           'name': 'foaf:name',
           'familyName': 'foaf:familyName',
           'Person': 'foaf:Person'
        },
        '@id': 'http://t-code.pl/#tomasz',
        '@type': 'Person',
        'name': 'Tomasz',
        'lastName': 'Pluskiewicz'
    }");

    // when
    IEntitySerializer serializer = new EntitySerializer(new StaticContextProvider());
    var person = serializer.Deserialize<Person>(json);

    // then
    Assert.That(person.Name, Is.EqualTo("Tomasz"));
    Assert.That(person.LastName, Is.EqualTo("Pluskiewicz"));
    Assert.That(person.Id, Is.EqualTo(new Uri("http://t-code.pl/#tomasz")));
}

}
```

[playground]: http://json-ld.org/playground/
[jsonld-spec]: http://json-ld.org/spec/latest/json-ld/
[jsonld]: http://json-ld.org
[rdf]: http://en.wikipedia.org/wiki/Resource_Description_Framework
[readme]: /wikibus/JsonLD.Entities/blob/master/src/JsonLD.Docu/Readme.cs
[jsonld-context]: http://www.w3.org/TR/json-ld/#the-context
