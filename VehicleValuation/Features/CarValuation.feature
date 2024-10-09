Feature: CarValuation




Scenario: Verify car valuation

Given user have a list of vehicle registration numbers from '<InputFilePath>'
    When user perform car valuation using the valuation website
    Then user should see the results in the output files
    And the output should match expected results '<ExpectedFilePath>'  
	
     Examples: 
        | InputFilePath     | ExpectedFilePath |  
        | C:\Users\srila\OneDrive\Desktop\InputFile.txt     | C:\Users\srila\OneDrive\Desktop\Expected\OutputFile.txt | 
  


