Feature: Command Parsing

Scenario: Invoke a command without arguments
	Given I have a command "test" which doesn't take arguments
	When I run my application with the args "test"
	Then the "test" command is invoked