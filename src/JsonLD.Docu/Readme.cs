/**
![icon](https://raw.githubusercontent.com/wikibus/JsonLD.Entities/master/assets/icon.png)
 
# JsonLD.Entities [![Build status](https://ci.appveyor.com/api/projects/status/u4riv8ftspthkvgh/branch/master?svg=true)](https://ci.appveyor.com/project/tpluscode78631/jsonld-entities/branch/master)
 
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

## Getting JsonLD.Entities

The project's CI build creates nupkgs automatically and they are published to a appveyor feed. To install use the below command in
Package Manager Console.

```
install-package jsonLD.Entities -Source https://ci.appveyor.com/nuget/jsonld-entities-aavhsnxi7xjp
```

## Building

The project was created in VS 2010 and should build without problems on any VS 2010 and newer. Note that external packages aren't
downloaded with NuGet, but rather with a new tool called [Paket](http://fsprojects.github.io/Paket/). For convenience there is a batch file
in repository root, which will restore the dependencies. For more information about Paket do visit its project page. It's actually very 
cool! :yum:

## Usage examples

This entire page is generated for actual C# code. You can view it in the [Readme.cs][readme] file and run the tests
when you open the solution. Each example below and on the wiki is a compiled and runnable unit test.

First let's import the required namespaces.
 **/

using System;
using JsonLD.Entities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

/**
### Deserialization

#### Deserializing the model as-is

The easiest operation possible is to deserialize a JSON-LD object without any changes. The example models will be deserialized to 
instances of a Person class.
 **/

[Class("http://xmlns.com/foaf/0.1/Person")]
public class Person
{
    public Uri Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }
}

/**
Serialization and deserialization is done by instances of `IEntitySerializer`. It's default implementation requires you to pass a 
IContextProvider, which provides [@context][jsonld-context] objects for serialized types. Because, we don't want to use a context in the
first test, the IContextProvider object won't be set up in any way.

Note how the JSON-LD `@id` is by convention deserialized to the `Person#Id` property.
**/

[TestFixture]
public class Deserialization
{

[Test]
public void Can_deserialize_with_existing_context()
{
    // given
    var json = JObject.Parse(@"
    {
        '@context': {
           'foaf': 'http://xmlns.com/foaf/0.1/',
           'name': 'foaf:name',
           'lastName': 'foaf:familyName',
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

/**
#### Deserialize with specific @context

Oftentimes, like in public API, you could receive models, which do not conform to some specific JSON structure. With JSON-LD it is possible,
becuase any document can be represented in numerous equivalent ways. For that purpose the [specification][jsonld-spec] defines a set of
[algorithms][jsonld-api], which can transform a JSON-LD document between those representations.

Below example shows how the default `IContextProvider` is used to adjust the document strucuture before deserializing. Perceptive reader
would have probably noticed already that the `@context` must conform to model's properties. Pascal case in c#, camel case in JSON.
**/

[Test]
public void Can_deserialize_with_changed_context()
{
    // given
    var expanded = JObject.Parse(@"
    {
        '@id': 'http://t-code.pl/#tomasz',
        '@type': 'http://xmlns.com/foaf/0.1/Person',
        'http://xmlns.com/foaf/0.1/name': 'Tomasz',
        'http://xmlns.com/foaf/0.1/familyName': 'Pluskiewicz'
    }");

    var @context = JObject.Parse(@"
    {
        'foaf': 'http://xmlns.com/foaf/0.1/',
        'name': 'foaf:name',
        'lastName': 'foaf:familyName',
        'Person': 'foaf:Person'
    }");

    var contextProvider = new StaticContextProvider();
    contextProvider.SetContext(typeof(Person), @context);

    // when
    IEntitySerializer serializer = new EntitySerializer(contextProvider);
    var person = serializer.Deserialize<Person>(expanded);

    // then
    Assert.That(person.Name, Is.EqualTo("Tomasz"));
    Assert.That(person.LastName, Is.EqualTo("Pluskiewicz"));
    Assert.That(person.Id, Is.EqualTo(new Uri("http://t-code.pl/#tomasz")));
}
}

/**
### Serialization

Of course it also possible to serialize POCO objects to JSON-LD objects. This time it is necessary to set up a `@context`, which will be
added to the result.
**/

[TestFixture]
public class Serialization
{

[Test]
public void Can_serialize_object_to_JSON_LD()
{
    // given
    var person = new Person
        {
            Id = new Uri("http://t-code.pl/#tomasz"),
            Name = "Tomasz",
            LastName = "Pluskiewicz"
        };
    var @context = JObject.Parse("{ '@context': 'http://example.org/context/Person' }");

    var contextProvider = new StaticContextProvider();
    contextProvider.SetContext(typeof(Person), @context);

    // when
    IEntitySerializer serializer = new EntitySerializer(contextProvider);
    dynamic json = serializer.Serialize(person);

    // then
    Assert.That((string)json.name, Is.EqualTo("Tomasz"));
    Assert.That((string)json.lastName, Is.EqualTo("Pluskiewicz"));
    Assert.That((string)json["@id"], Is.EqualTo("http://t-code.pl/#tomasz"));
    Assert.That((string)json["@type"][0], Is.EqualTo("http://xmlns.com/foaf/0.1/Person"));
    Assert.That(json["@context"], Is.Not.Null);
}
}

/**
Graph image from [W3C](http://www.w3.org/RDF/) originally desgined by [Bill Schwappacher](mailto:bill@tracermedia.com).

[playground]: http://json-ld.org/playground/
[jsonld-spec]: http://json-ld.org/spec/latest/json-ld/
[jsonld-api]: http://www.w3.org/TR/json-ld-api/
[jsonld]: http://json-ld.org
[rdf]: http://en.wikipedia.org/wiki/Resource_Description_Framework
[readme]: http://github.com/wikibus/JsonLD.Entities/blob/master/src/JsonLD.Docu/Readme.cs
[jsonld-context]: http://www.w3.org/TR/json-ld/#the-context
**/