Feature: Argument Binding

Scenario: Bind string parameter
	Given I have an application which requires the following arguments
	| name | type   |
	| name | String |
	When I run my application with the args "FooBar"
	Then the application handler is invoked with the values
	| name | type   | value  |
	| name | String | FooBar |

Scenario: Bind integer parameter
	Given I have an application which requires the following arguments
	| name | type  |
	| id   | Int32 |
	When I run my application with the args "12345"
	Then the application handler is invoked with the values
	| name | type  | value |
	| id   | Int32 | 12345 |

Scenario: Too many arguments
	Given I have an application which requires the following arguments
	| name | type  |
	| id   | Int32 |
	When I run my application with the args "12345 678910"
	Then the application handler is invoked with the values
	| name | type  | value |
	| id   | Int32 | 12345 |

Scenario: Too few arguments
	Given I have an application which requires the following arguments
	| name  | type  |
	| id    | Int32 |
	| count | Int32 |
	When I run my application with the args "12345"
	Then the application prints the error "missing required parameter 'count'"

Scenario: Try bind non-numeric to Int32
	Given I have an application which requires the following arguments
	| name | type  |
	| id   | Int32 |
	When I run my application with the args "potato"
	Then the application prints the error "cannot use 'potato' as the 'id' parameter; could not convert to System.Int32"

Scenario: Try bind decimal to Int32
	Given I have an application which requires the following arguments
	| name | type  |
	| id   | Int32 |
	When I run my application with the args "123.456"
	Then the application prints the error "cannot use '123.456' as the 'id' parameter; could not convert to System.Int32"

Scenario: Try bind non-numeric to Double
	Given I have an application which requires the following arguments
	| name | type   |
	| size | Double |
	When I run my application with the args "potato"
	Then the application prints the error "cannot use 'potato' as the 'size' parameter; could not convert to System.Double"