Feature: Deserializing RDF data into objects

@NQuads
Scenario: Deserialize simple resource entity
	Given NQuads:
		"""
		<http://example.com/Person> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://example.com/ontology#Person> .
		<http://example.com/Person> <http://xmlns.com/foaf/0.1/givenName> "Tomasz" .
		<http://example.com/Person> <http://xmlns.com/foaf/0.1/familyName> "Pluskiewicz" .
		<http://example.com/Person> <http://example.com/ontology#dateOfBirth> "1975-08-15" .
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
