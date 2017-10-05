Feature: Verbs

Scenario: Invoke a verb without arguments
	Given I have a handler for the verb "test" which doesn't take arguments
	When I run my application with the args "test"
	Then the "test" handler is invoked

Scenario: Invoke a verb with a string argument
	Given I have a handler for the verb "test" which requires the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "test FooBar"
	Then the "test" handler is invoked with the following arguments
	| name | type   | value  |
	| name | String | FooBar |

Scenario: Invoke a verb with an integer argument
	Given I have a handler for the verb "test" which requires the following arguments
	| name | type   |
	| size | Int32 |
	When I run my application with the args "test 12345"
	Then the "test" handler is invoked with the following arguments
	| name | type  | value |
	| size | Int32 | 12345 |

Scenario: Invoke a verb with a decimal argument
	Given I have a handler for the verb "test" which requires the following arguments
	| name | type   |
	| size | Double |
	When I run my application with the args "test 12345.6789"
	Then the "test" handler is invoked with the following arguments
	| name | type   | value      |
	| size | Double | 12345.6789 |

Scenario: Invoke a verb with too many arguments
	Given I have a handler for the verb "test" which requires the following arguments
	| name | type   |
	| size | Int32 |
	When I run my application with the args "test 12345 678910"
	Then the "test" handler is invoked with the following arguments
	| name | type  | value |
	| size | Int32 | 12345 |

Scenario: Invoke a verb with too few arguments
	Given I have a handler for the verb "test" which requires the following arguments
	| name  | type  |
	| size  | Int32 |
	| count | Int32 |
	When I run my application with the args "test 12345"
	Then the application prints the error "missing parameter 'count'"