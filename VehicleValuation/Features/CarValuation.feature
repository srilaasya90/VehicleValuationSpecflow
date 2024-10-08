Feature: CarValuation




#@DataSource:FilePath.xlsx
Scenario: Verify car valuation
Given user have a list of vehicle registration numbers from "C:\Users\srila\OneDrive\Desktop\InputFile.txt"
    When user perform car valuation using the valuation website
    Then user should see the results in the output files
    And the output should match expected results for "C:\Users\srila\OneDrive\Desktop\OutputFile.txt" and "C:\Users\srila\OneDrive\Desktop\OutputFile.txt"
	
