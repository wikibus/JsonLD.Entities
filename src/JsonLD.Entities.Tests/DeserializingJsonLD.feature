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
