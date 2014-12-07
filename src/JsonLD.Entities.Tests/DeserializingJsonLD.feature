Feature: DeserializingJsonLD
    In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers

@JsonLD
Scenario: Deserialize compacted JSON-LD object
    Given JSON-LD:
        """
        {
            "@context": {
                "foaf": "http://xmlns.com/foaf/0.1/",
                "firstName": "foaf:givenName",
                "lastName": "foaf:familyName"
            },
            "@id": "http://example.com/Person",
            "firstName": "Tomasz",
            "lastName": "Pluskiewicz",
            "http://example.com/ontology#dateOfBirth": "1975-08-15"
        }
        """
    And @context is:
        """
        {
            foaf: "http://xmlns.com/foaf/0.1/",
            name: "foaf:givenName",
            surname: "foaf:familyName",
            birthDate: "http://example.com/ontology#dateOfBirth"
        }
        """
    When I deserialize into 'JsonLD.Entities.Tests.Entities.Person'
    Then object should have property 'Name' equal to 'Tomasz'
    And object should have property 'Surname' equal to 'Pluskiewicz'
    And object should have DateTime property 'BirthDate' equal to '15-08-1975'

@JsonLD
Scenario Outline: Deserialize single element into collection
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": "RDF"
        }
        """
    And @context is:
        """
        {
            "foaf": "http://xmlns.com/foaf/0.1/",
            "interests": { "@id": "foaf:topic_interest", "@container": "<container>" }
        }
        """
    When I deserialize into '<type>'
    Then object should have property 'Interests' containg string 'RDF'
    Examples: 
    | type                                                  | container |
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      | @list     |
    | JsonLD.Entities.Tests.Entities.HasInterestsList       | @list     |
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable | @list     |
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection | @list     |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        | @list     |
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      | @set      |
    | JsonLD.Entities.Tests.Entities.HasInterestsList       | @set      |
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable | @set      |
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection | @set      |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        | @set      |
    
@JsonLD
Scenario Outline: Deserialize single element into array when there's no @context
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": "RDF"
        }
        """
    When I deserialize into '<type>'
    Then object should have property 'Interests' containg string 'RDF'
    Examples: 
    | type                                                  |
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      |
    | JsonLD.Entities.Tests.Entities.HasInterestsList       |
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable |
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection |
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        |