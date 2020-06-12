Feature: DocumentApi
	As a publisher
	I would like to upload, manually re-order, download and delete pdf's
	So, I can place a list of documents on my client apps and website for users to download
	And in an arbitrary order of my choosing

Scenario: Pdf Upload
	Given I have a PDF to upload
	When I send the file to the API
	Then it is uploaded successfully
 
Scenario: Non-pdf file upload rejection
	Given I have a non-pdf to upload
	When I send the file to the API
	Then the API does not accept the file and returns the appropriate messaging and status

