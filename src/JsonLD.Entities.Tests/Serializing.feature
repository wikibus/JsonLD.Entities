Feature: Serializing
	Test serializing models to JSON-LD

@mytag
Scenario: Serialize simple model with blank id
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
