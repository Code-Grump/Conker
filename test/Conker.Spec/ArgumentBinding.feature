Feature: Argument Binding

Scenario: Bind string parameter
	Given I have an application which takes the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "FooBar"
	Then the application handler is invoked with the values
	| name | type   | value  |
	| name | String | FooBar |

Scenario: Bind integer parameter
	Given I have an application which takes the following arguments
	| name | type  |
	| id   | Int32 |
	When I run my application with the args "12345"
	Then the application handler is invoked with the values
	| name | type  | value |
	| id   | Int32 | 12345 |
