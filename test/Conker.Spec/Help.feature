Feature: Help

Scenario: Get help for application without verbs
	Given I have an application "time" which takes the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "--help"
	Then the application prints usage information "time <name>"