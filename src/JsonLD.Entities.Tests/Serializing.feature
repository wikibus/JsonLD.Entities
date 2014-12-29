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
            "@type": [ "ex:Person" ]
        }
        """