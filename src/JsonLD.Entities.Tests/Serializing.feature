Feature: Serializing
    Test serializing models to JSON-LD

Scenario: Serialize simple model with blank id
    Given a person without id
    When the object is serialized
    Then the resulting JSON-LD should be:
        """
        {
            "name": "Tomasz",
            "surname": "Pluskiewicz",
            "birthDate": "1972-09-04T00:00:00"
        }
        """

Scenario Outline: Serialize model with single element in set
    Given model of type '<type>'
      And model has interest 'RDF'
     When the object is serialized
     Then the resulting JSON-LD should be:
        """
        {
            "interests": [ "RDF" ]
        }
        """
 Examples:
    | type                                                  |
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        |
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection |
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable |
    | JsonLD.Entities.Tests.Entities.HasInterestsGenerator  |

Scenario: Serialize model with single element in list
    Given model of type 'JsonLD.Entities.Tests.Entities.HasInterestsList'
      And model has interest 'RDF'
     When the object is serialized
     Then the resulting JSON-LD should be:
        """
        {
            "interests": [ "RDF" ]
        }
        """

Scenario Outline: Serialize model with empty collection
    Given model of type '<type>'
     When the object is serialized
     Then the resulting JSON-LD should be:
        """
        {
        }
        """
 Examples:
    | type                                                 |
    | JsonLD.Entities.Tests.Entities.HasInterestsGenerator |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet       |

Scenario: Serialize model with prefixed name in ClassAttribute
    Given model of type 'JsonLD.Entities.Tests.Entities.PersonWithPrefixedClass'
    And @context is:
         """
         {
            "ex": "http://example.com/ontology#"
         }
         """
    When the object is serialized
    Then the resulting JSON-LD should be:
        """
        {
            "@type": "ex:Person"
        }
        """
		
Scenario: Serializing primitive values should produce short literals for Boolean, Double and String
	Given model of type 'JsonLD.Entities.Tests.Entities.AllPrimitives'
	| Property | Value |
	| string   | Hello |
	| double   | 3.14  |
	| bool     | true  |
	When the object is serialized
	Then the resulting JSON-LD should be:
         """
         {
			"string": "Hello",
			"double": 3.14,
			"bool": true
		 }
         """

Scenario Outline: Serializing primitive values should produce typed literals
	Given model of type 'JsonLD.Entities.Tests.Entities.AllPrimitives'
	| Property   | Value   |
	| <Property> | <Value> |
	When the object is serialized
	Then the resulting JSON-LD should be:
         """
         {
			"<Property>": {
				"@value": "<JsonValue>",
				"@type": "http://www.w3.org/2001/XMLSchema#<Property>"
			}
		 }
         """
	Examples: 
	| Property | Value      | JsonValue           | XsdType       |
	| date     | 2016-01-03 | 2016-01-03T00:00:00 | dateTime      |
	| decimal  | 3.4        | 3.4                 | decimal       |
	| long     | 100        | 100                 | long          |
	| ulong    | 100        | 100                 | unsignedLong  |
	| int      | 1567       | 1567                | int           |
	| uint     | 15         | 15                  | unsignedInt   |
	| short    | 17         | 17                  | short         |
	| ushort   | 3          | 3                   | unsignedShort |
	| byte     | 20         | 20                  | unsignedByte  |
	| sbyte    | -3         | -3                  | byte          |
	| float    | 2.3456     | 2.3456              | float         |
	| timeSpan | 100        | 100                 | duration      |