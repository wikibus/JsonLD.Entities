![icon](https://raw.githubusercontent.com/wikibus/JsonLD.Entities/master/assets/icon.png)
 
# JsonLD.Entities [![Build status](https://ci.appveyor.com/api/projects/status/u4riv8ftspthkvgh/branch/master?svg=true)](https://ci.appveyor.com/project/tpluscode78631/jsonld-entities/branch/master)
[![NuGet version](https://badge.fury.io/nu/jsonld.entities.svg)](https://badge.fury.io/nu/jsonld.entities)
 
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

The project's CI build creates nupkgs automatically and they are published on NuGet. To install use the below command in
Package Manager Console.

```
install-package JsonLD.Entities
```

Prerelease builds are available on AppVeyor's project feed: `https://ci.appveyor.com/nuget/jsonld-entities-aavhsnxi7xjp`

## Building

The project was created in VS 2010 and should build without problems on any VS 2010 and newer. Note that external packages aren't
downloaded with NuGet, but rather with a new tool called [Paket](http://fsprojects.github.io/Paket/). For a better developer eperience
there is a [Visual Studio Extension](https://visualstudiogallery.msdn.microsoft.com/ce104917-e8b3-4365-9490-8432c6e75c36). 
Otherwise run this in console from repository root:

``` batch
> .paket\paket.bootstrapper.exe
> .paket\paket.exe restore
```

## Usage examples

All usage samples are written in the [Literate Programming][lp] manner and can be conveniently viewed on GitHub in the [Documentation project][docs]

Graph image from [W3C](http://www.w3.org/RDF/) originally desgined by [Bill Schwappacher](mailto:bill@tracermedia.com).

[playground]: http://json-ld.org/playground/
[jsonld-spec]: http://json-ld.org/spec/latest/json-ld/
[jsonld-api]: http://www.w3.org/TR/json-ld-api/
[jsonld]: http://json-ld.org
[rdf]: http://en.wikipedia.org/wiki/Resource_Description_Framework
[readme]: http://github.com/wikibus/JsonLD.Entities/blob/master/src/Documentation/Readme.cs
[jsonld-context]: http://www.w3.org/TR/json-ld/#the-context
[docs]: https://github.com/wikibus/JsonLD.Entities/tree/master/src/Documentation
[lp]: https://en.wikipedia.org/wiki/Literate_programming
