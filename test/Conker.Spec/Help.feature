Feature: Help

Scenario: Get help by default for application without verbs
	Given I have an application "time" which takes the following arguments
	| name | type   |
	| name | String |
	When I run my application with no args
	Then the application prints usage information "time <name>"

Scenario: Get help for application without verbs
	Given I have an application "time" which takes the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "--help"
	Then the application prints usage information "time <name>"

Scenario: Get help by default for an application with verbs
	Given my application is called "time"
	And I have a handler for the verb "get" which doesn't take arguments
	Given I have a handler for the verb "set" which takes the following arguments
	| name | type  |
	| now  | Int32 |
	When I run my application with no args
	Then the application prints usage information "time <command> [<args>]" with the following commands
	| name |
	| get  |
	| set  |

Scenario: Get help for a verb
	Given my application is called "time"
	And I have a handler for the verb "get" which doesn't take arguments
	Given I have a handler for the verb "set" which takes the following arguments
	| name | type  |
	| now  | Int32 |
	When I run my application with the args "set --help"
	Then the application prints usage information "time set <now>"
