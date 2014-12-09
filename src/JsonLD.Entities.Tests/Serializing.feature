Feature: Serializing
    Test serializing models to JSON-LD

Scenario: Serialize simple model with blank id
    Given a person without id
    When the object is serialized
    Then the resulting JSON-LD should be:
        """
        {
            "@context": {
                "foaf": "http://xmlns.com/foaf/0.1/",
                "name": "foaf:givenName",
                "surname": "foaf:familyName",
                "birthDate": "http://example.com/ontology#dateOfBirth"
            },
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
            "@context": {
               "foaf": "http://xmlns.com/foaf/0.1/",
               "interests": { "@id": "foaf:topic_interest", "@container": "@set" }
            }
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": "RDF"
        }
        """
 Examples:
    | type                                                  |
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        |
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection |
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable |