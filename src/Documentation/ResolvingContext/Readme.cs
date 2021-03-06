﻿/**
# Documentation

## Defining entities's @context and @frame

To correctly serialize and deserialize JSON-LD documents into given models it may be necessary to process them with a `@context` or `@frame`
so that it's structure conforms to a given schema. This page lists the ways in which it is possible to define `@context` and `@frame` for
the C# types.

First let's import the required namespaces.
 **/

using System;
using JsonLD.Entities;
using Newtonsoft.Json.Linq;
using Xunit;

/**
### Define `@context` inline with class definition
**/

public class DefiningContextInline
{

private const string TestContext = "{ '@base': 'http://example.com/' }";

/**
The simples way is to define a `Context` property. It must be static and return a string or `JToken`. It can also be private.
    
Below are example entity types with various ways of defining the `@context` as property.
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

/**
When each of the above will be serailized, the inline context will be used.
**/

[Theory]
[InlineData(typeof(ContextInlineProperty))]
[InlineData(typeof(ContextInlinePrivateProperty))]
[InlineData(typeof(ContextInlineStaticStringProperty))]
public void UsesInlineContextPropertyWhenSerializing(Type entityType)
{
    // given
    const string expected = "{ '@context': { '@base': 'http://example.com/' } }";
    var entity = Activator.CreateInstance(entityType);
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.True(JToken.DeepEquals(json, JObject.Parse(expected)), $"Actual object is {json}");
}

/**
Finally, the type can declare a static `GetContext` method, which takes an object of said type as parameter.
This way a `@context` can be built dynamically.

Note that when both property and method is present, the mthod will be preferred.
 **/

public class ContextInlineMethod
{
    private readonly string _base;

    public ContextInlineMethod(string @base)
    {
        _base = @base;
    }

    private static JObject GetContext(ContextInlineMethod instance)
    {
        return JObject.Parse($@"{{
            '@base': 'http://example.com/{instance._base}/'
        }}");
    }
    }

[Fact]
public void UsesGetContextMethodSerializing()
{
    // given
    const string expected = "{ '@context': { '@base': 'http://example.com/test/' } }";
    var entity = new ContextInlineMethod("test");
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.True(JToken.DeepEquals(json, JObject.Parse(expected)), $"Actual object is {json}");
}
}