/**
# Documentation

## Defining entities's @context and @frame

To correctly serialize and deserialize JSON-LD documents into given models it may be necessary to process them with a `@context` or `@frame`
so that it's structure conforms to a given schema. This page lists the ways in which it is possible to define `@context` and `@frame` for
the C# types.

First let's import the required namespaces.
 **/

using System;
using System.Globalization;
using JsonLD.Entities;
using JsonLD.Entities.Context;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

/**
### Define `@context` inline with class definition
**/

[TestFixture]
public class DefiningContextInline
{

private const string TestContext = "{ '@base': 'http://example.com/' }";

/**
The simples way is to define a `Context` property. It must be static and return a string or `JToken`. It can also be private. Another way
is to declare a `GetContext` method, which takes an object as parameter. This way a @context can be built dynamically.
    
Below are example entity types with various ways of defining the `@context` inline.
**/

public class ContextInlineProperty
{
    public static JObject Context
    {
        get { return JObject.Parse(TestContext); }
    }
}

public class ContextInlinePrivateProperty
{
    private static JObject Context
    {
        get { return JObject.Parse(TestContext); }
    }
}

public class ContextInlineStaticStringProperty
{
    public static string Context
    {
        get { return TestContext; }
    }
}

public class ContextInlineMethod
{
    private static JObject GetContext(ContextInlineMethod instance)
    {
        return JObject.Parse(TestContext);
    }
}

/**
When each of the above will be serailized, the inline context will be used.
**/

[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlinePrivateProperty))]
[TestCase(typeof(ContextInlineStaticStringProperty))]
[TestCase(typeof(ContextInlineMethod))]
public void UsesInlineContextWhenSerializing(Type entityType)
{
    // given
    const string expected = "{ '@context': { '@base': 'http://example.com/' } }";
    var entity = Activator.CreateInstance(entityType);
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.That(JToken.DeepEquals(json, JObject.Parse(expected)), "Actual object is {0}", json);
}

/**
### Building the `@context` programmatically

The most reasonable way for creating the `@context` is to produce a `JObject` (or `JArray`) with the desired structure programmatically.
This way it would also be possible to reuse and modify common contexts shared by hierarchies of classes. To simplify the repetitive chore
of creating complex objects and introduce some semantics into the code, JsonLd.Entities introduces specialized classes for parts of the
`@context` strucutre. They make it simpler to use advanced features like [type coercion][coercion], [internationalization][i8n] 
or [@reverse][reverse].

This can be done simply by passing instances of the aforementioned classes to a JObject constructor or appending them to
**/

[Test]
public void BuildComplexContextSimply()
{
    // given
    const string expected = @"
{
    '@base': 'http://example.com/',
    '@vocab': 'http://schema.org/',
    'dcterms': 'http://purl.org/dc/terms/',
    'xsd': 'http://www.w3.org/2001/XMLSchema#',
    'title': 'dcterms:title',
    'creator': { 
        '@id': 'dcterms:creator', 
        '@type': '@id'
    },
    'medium': { 
        '@id': 'dcterms:medium', 
        '@container': '@set', 
        '@type': '@vocab'
    },
    'date': { 
        '@id': 'dcterms:date', 
        '@type': 'xsd:date'
    },
    '@language': 'en',
    'label': {
        '@id': 'http://www.w3.org/2004/02/skos/core#prefLabel',
        '@language': null
    },
    'altLabel': {
        '@id': 'http://www.w3.org/2004/02/skos/core#altLabel',
        '@container': '@language',
        '@type': 'xsd:string'
    }
}";

    // when
    var context = new JObject(
        Base.Is("http://example.com/"),
        Vocab.Is(new Uri("http://schema.org/")),
        "dcterms".IsPrefixOf("http://purl.org/dc/terms/"),
        "xsd".IsPrefixOf(new Uri("http://www.w3.org/2001/XMLSchema#")),
        "title".IsProperty("dcterms:title"),
        "creator".IsProperty("dcterms:creator")
                 .Type().Id(),
        "medium".IsProperty("dcterms:medium")
                .Container().Set()
                .Type().Vocab(),
        "date".IsProperty("dcterms:date")
              .Type().Is("xsd:date"),
        "en".IsLanguage(),
        "label".IsProperty("http://www.w3.org/2004/02/skos/core#prefLabel")
               .Language(null),
        "altLabel".IsProperty("http://www.w3.org/2004/02/skos/core#altLabel")
                  .Container().Language()
                  .Type().Is("xsd:string"));

    // then
    Assert.That(JToken.DeepEquals(context, JObject.Parse(expected)), "Actual context was {0}", context);
}
}

/**
[coercion]: http://www.w3.org/TR/json-ld/#type-coercion
[reverse]: http://www.w3.org/TR/json-ld/#reverse-properties
[i8n]: http://www.w3.org/TR/json-ld/#string-internationalization
**/