﻿Feature: Serializing
	Test serializing models to JSON-LD

@mytag
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
			"birthDate": "1972-09-04"
		}
		"""