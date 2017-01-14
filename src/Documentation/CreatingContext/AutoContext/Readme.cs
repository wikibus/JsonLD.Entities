﻿/**
# Documentation

## Building the `@context` shorthands

The JSON-LD `@context` can be build manually from scratch, but in some cases
it may be possible to reduce the context to some common base for each property
and set it up accordingly.

Currently there are two strategies for creating terms for properties.

### Class identifier as base

First way is to concatenate the class identifier with property names. This can be done
in two ways:

1. If the type is a hash URI, append to the hash fragment:

    `http://example.com/vocab#Person` -> `http://example.com/vocab#Person/propertyName`

1. Otherwise append the property name as hash fragment:

    `http://example.com/vocab/Person` -> `http://example.com/vocab/Person#propertyName`
 **/

using JsonLD.Entities.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

[TestFixture]
public class AutomaticContextBuilding
{
    private class Person
    {
        private static string Type => "http://example.com/vocab/Person";

        public string Name { get; set; }

        // also works with custom property names
        [JsonProperty("lastName")]
        public string Surname { get; set; }

        public static JObject Context => new AutoContext<Person>();
    }

    [Test]
    public void Context_can_be_built_from_type_id()
    {
        // given
        dynamic context = Person.Context;

        // then
        Assert.That((string)context.name, Is.EqualTo("http://example.com/vocab/Person#name"));
        Assert.That((string)context.lastName, Is.EqualTo("http://example.com/vocab/Person#lastName"));
    }
}

/**

Note that is if the class type doesn't have a statically resolvable Type identifier
(that is, using a static property or annotation), then it will be necessary to create
`AutoContext` with a constructor which takes URL as one of its parameter.
 
**/ 

/**

### Vocabulary IRI as base

### Combining with existing context

### Custom automatic context

If you ever find the need to implement a different logic for generating the 
`@context` you can implement the abstract [`AutoContextBase&lt;T&gt;`][acb] class

[acb]: https://github.com/wikibus/JsonLD.Entities/blob/master/src/JsonLD.Entities/Context/AutoContextBase.cs

**/