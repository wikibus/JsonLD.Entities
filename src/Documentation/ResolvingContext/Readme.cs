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
using NUnit.Framework;

/**
### Define `@context` inline with class definition
**/

[TestFixture]
public class DefiningContextInline
{

private const string Context = "{ '@base': 'http://example.com/' }";

/**
The simples way is to define a `_context` property. It must return a string or `JToken` and can be static and private. Below are
example entity types with various ways of defining the `@context`.
**/

public class ContextInlineProperty
{
    public JObject _context
    {
        get { return JObject.Parse(Context); }
    }
}

public class ContextInlinePrivateProperty
{
    private JObject _context
    {
        get { return JObject.Parse(Context); }
    }
}

public class ContextInlineStaticStringProperty
{
    public static string _context
    {
        get { return Context; }
    }
}

public class ContextInlinePrivateStaticProperty
{
    private static JObject _context
    {
        get { return JObject.Parse(Context); }
    }
}

/**
When each of the above will be serailized, the inline context will be used.
**/

[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlinePrivateProperty))]
[TestCase(typeof(ContextInlineStaticStringProperty))]
[TestCase(typeof(ContextInlinePrivateStaticProperty))]
public void UsesInlineContextWhenSerializing(Type entityType)
{
    // given
    const string expected = "{ '@context': { '@base': 'http://example.com' } }";
    var entity = Activator.CreateInstance(entityType);
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.That(JToken.DeepEquals(json, JObject.Parse(expected)));
}
}