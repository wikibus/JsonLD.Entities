Feature: DeserializingJsonLD
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Deserialize compacted JSON-LD object
	Given JSON-LD:
		"""
		{
			"@context": {
				foaf: "http://xmlns.com/foaf/0.1/",
				name: "foaf:givenName",
				surname: "foaf:familyName",
				birthDate: "http://example.com/ontology#dateOfBirth"
			},
			"@id": "http://example.com/Person",
			name: "Tomasz",
			surname: "Pluskiewicz",
			birthDate: "1975-08-15"
		}
		"""
	When I deserialize into 'JsonLD.Entities.Tests.Entities.Person'
	Then object should have property 'Name' equal to 'Tomasz'
	And object should have property 'Surname' equal to 'Pluskiewicz'
	And object should have DateTime property 'BirthDate' equal to '15-08-1975'
