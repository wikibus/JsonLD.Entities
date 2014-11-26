/**
 * [![Build status](https://ci.appveyor.com/api/projects/status/u4riv8ftspthkvgh/branch/master?svg=true)](https://ci.appveyor.com/project/tpluscode78631/jsonld-entities/branch/master)
 * 
 * # JsonLD.Entities
 * 
 * JsonLD.Entities is a small library for serializing and deserializing POCOs in JSON-LD.
 * 
 * ## What is JSON-LD?
 * 
 * If you are not familiar with JSON-LD you could first go to it's [main website][jsonld].
 * 
 * JSON-LD is the newest [RDF][rdf] serialization format. It means that serialized 
 * data has structure and unambiguous meaning. For example if there is a `name` property, it can be identified becasue the `name` relation 
 * itself can be identified by the use of URI (and usually URLs).
 * 
 * In general JSON-LD has a familiar json structure, which makes it best suited as the entrypoint to the Semantic Web at large. It works by 
 * simply extending the typical json document structure. In reality it gets a _little_ bit more complex, because json has a tree structure,
 * but RDF represents graphs. And it is possible to represent a graph as multiple equivalent trees.
 * 
 * To learn more about JSON-LD you should visit its [formal specification][jsonld-spec] and the [playground][playground], where you can 
 * experiment.
 * 
 * ## Using JsonLD.Entities
 * 
 * 
 * 
 * ### Deserialization
 * 
 * Let's create our input document
 **/
 
JObject tomasz = JObject.Parse(@"{
    "@context": {
       "name": "http://xmlns.com/foaf/0.1/name",
       "familyName": "http://xmlns.com/foaf/0.1/familyName"
    },
    "@id": "http://t-code.pl/#tomasz",
    "name": "Tomasz",
    "lastName": "Pluskiewicz"
}");

/**
 * [playground]: http://json-ld.org/playground/
 * [jsonld-spec]: http://json-ld.org/spec/latest/json-ld/
 * [jsonld]: http://json-ld.org
 * [rdf]: http://en.wikipedia.org/wiki/Resource_Description_Framework
**/