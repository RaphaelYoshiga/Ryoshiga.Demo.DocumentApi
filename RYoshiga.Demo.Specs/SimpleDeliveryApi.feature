Feature: SimpleDeliveryApi
	In order to know what delivery options I have for my order
	As a customer
	I want to be shown delivery options

Scenario: Simple delivery options
	Given I have those delivery options for country code GB
		| Name              | Price | DaysToDispatch | DaysToDeliver |
		| Standard Delivery | 3.00  | 2              | 2             |
		| Next Day Delivery | 3.00  | 0              | 1             |
	And the time is 01/01/2020 20:00
	When I ask for delivery options for GB
	Then I get those delivery options
		| Name              | Price | DeliveryDate |
		| Standard Delivery | 3.00  | 05/01/2020   |
		| Next Day Delivery | 3.00  | 02/01/2020   |

Scenario: Delivery options for France
	Given I have those delivery options for country code FR
		| Name                   | Price | DaysToDispatch | DaysToDeliver |
		| International Delivery | 7.00  | 0              | 4             |
	And the time is 01/01/2020 20:00
	When I ask for delivery options for fr
	Then I get those delivery options
		| Name                   | Price | DeliveryDate |
		| International Delivery | 7.00  | 05/01/2020   |
