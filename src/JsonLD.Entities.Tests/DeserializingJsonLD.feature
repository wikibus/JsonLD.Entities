Feature: DeserializingJsonLD

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
Scenario Outline: Deserialize single element into set
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
            "interests": { "@id": "foaf:topic_interest", "@container": "@set" }
        }
        """
    When I deserialize into '<type>'
    Then object should have property 'Interests' containg string 'RDF'
    Examples: 
    | type                                                  | 
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      | 
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable | 
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection | 
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        | 

@JsonLD
Scenario Outline: Deserialize array into set
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": [ "RDF", "SPARQL", "OWL" ]
        }
        """
    And @context is:
        """
        {
            "foaf": "http://xmlns.com/foaf/0.1/",
            "interests": { "@id": "foaf:topic_interest", "@container": "@set" }
        }
        """
    When I deserialize into '<type>'
    Then object should have property 'Interests' containg string 'RDF'
     And object should have property 'Interests' containg string 'SPARQL'
     And object should have property 'Interests' containg string 'OWL'
    Examples: 
    | type                                                  | 
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      | 
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable | 
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection | 
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        | 

@JsonLD
Scenario: Deserialize list
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": { "@list": [ "RDF", "SPARQL" ] }
        }
        """
    And @context is:
        """
        {
            "foaf": "http://xmlns.com/foaf/0.1/",
            "interests": { "@id": "foaf:topic_interest", "@container": "@list" }
        }
        """
    When I deserialize into 'JsonLD.Entities.Tests.Entities.HasInterestsList'
    Then object should have property 'Interests' containg string 'RDF'
     And object should have property 'Interests' containg string 'SPARQL'

@JsonLD
Scenario: Deserialize list when @container isn't specified
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": { "@list": [ "RDF", "SPARQL" ] }
        }
        """
    And @context is:
        """
        {
            "foaf": "http://xmlns.com/foaf/0.1/",
            "interests": { "@id": "foaf:topic_interest" }
        }
        """
    When I deserialize into 'JsonLD.Entities.Tests.Entities.HasInterestsList'
    Then object should have property 'Interests' containg string 'RDF'
     And object should have property 'Interests' containg string 'SPARQL'

@JsonLD
Scenario Outline: Deserialize single element into collection when @container isn't specified
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
            "interests": { "@id": "foaf:topic_interest" }
        }
        """
    When I deserialize into '<type>'
    Then object should have property 'Interests' containg string 'RDF'
    Examples: 
    | type                                                  | 
    | JsonLD.Entities.Tests.Entities.HasInterestsArray      | 
    | JsonLD.Entities.Tests.Entities.HasInterestsEnumerable | 
    | JsonLD.Entities.Tests.Entities.HasInterestsCollection | 
    | JsonLD.Entities.Tests.Entities.HasInterestsSet        | 

@JsonLD
Scenario: Deserialize list into set
    Given JSON-LD:
        """
        {
            "@id": "http://example.com/Person",
            "http://xmlns.com/foaf/0.1/topic_interest": { "@list": [ "RDF", "SPARQL" ] }
        }
        """
    And @context is:
        """
        {
            "foaf": "http://xmlns.com/foaf/0.1/",
            "interests": { "@id": "foaf:topic_interest", "@container": "@set" }
        }
        """
    When I deserialize into 'JsonLD.Entities.Tests.Entities.HasInterestsSet'
    Then Should fail

@JsonLD
Scenario: Deserialize a graph of objects
    Given JSON-LD:
         """
            [
              {
                "@id": "_:autos1",
                "http://schema.org/name": [
                  {
                    "@value": "Siegfried Bufe"
                  }
                ]
              },
              {
                "@id": "http://wikibus.org/book/6",
                "@type": [
                    "http://wikibus.org/ontology#Book"
                ],
                "http://schema.org/author": [
                  {
                    "@id": "_:autos1"
                  }
                ]
              }
            ]
         """
      And @context is:
         """
         {
           "sch": "http://schema.org/",
           "author": "sch:author",
           "name": "sch:name"
         }
         """
      And frame is
         """
         {
            "@type": "http://wikibus.org/ontology#Book"
         }
         """
     When I deserialize into 'JsonLD.Entities.Tests.Entities.Book'
     Then object should have object property 'Author'
      And object 'Author' should have property 'Name' equal to 'Siegfried Bufe'