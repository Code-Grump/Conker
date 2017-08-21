Feature: Command Parsing

Scenario: Invoke a command without arguments
	Given I have a handler for the command "test" which doesn't take arguments
	When I run my application with the args "test"
	Then the "test" command is invoked

Scenario: Invoke a command with a string argument
	Given I have a handler for the command "test" which takes the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "test FooBar"
	Then the "test" command is invoked with the following arguments
	| name | type   | value  |
	| name | String | FooBar |