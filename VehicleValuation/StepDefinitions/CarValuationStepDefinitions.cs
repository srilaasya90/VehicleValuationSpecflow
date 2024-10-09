using OpenQA.Selenium;
using System.Text.RegularExpressions;
using VehicleValuation.PageObjects;
using VehicleValuation.Models;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace VehicleValuation.StepDefinitions
{
    [Binding]
    public class CarValuationStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly CarValuationPage _valuationPage;
        private List<string> vehicleNumbers;
        private readonly string outputFilePath = @"C:\Users\srila\OneDrive\Desktop\OutputFile.txt";
        private  List<ValuationDetails> valuationDetailsList = new List<ValuationDetails>();
        public CarValuationStepDefinitions(IWebDriver driver, CarValuationPage valuationPage)
        {
            _driver = driver;
            _valuationPage = valuationPage;
        }

       

        [When(@"user perform car valuation using the valuation website")]
        public void WhenUserPerformCarValuationUsingTheValuationWebsite()
        {
         
            foreach (string vehicleNumber in vehicleNumbers)
            {
                _driver.Navigate().GoToUrl("https://www.webuyanycar.com");
                Thread.Sleep(10000);
                _valuationPage.EnterRegistrationNumber(vehicleNumber);
                _valuationPage.SubmitForm();             
                ValuationDetails valuationDetails = _valuationPage.GetValuation();
                valuationDetailsList.Add(valuationDetails);

            }
           
        }

        [Then(@"user should see the results in the output files")]
        public void ThenUserShouldSeeTheResultsInTheOutputFiles()
        {
            if (!File.Exists(outputFilePath))
            {
                throw new FileNotFoundException("Output file not found.");
            }
        }

        public List<ValuationDetails> ReadCsvFile(string outputFilePath)
        {
            using (var reader = new StreamReader(outputFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
               return csv.GetRecords<ValuationDetails>().ToList();
            }

        }

        [Given(@"user have a list of vehicle registration numbers from '([^']*)'")]
        public void GivenUserHaveAListOfVehicleRegistrationNumbersFrom(string inputFilePath)
        {
            if (File.Exists(inputFilePath))
            {
                string text = File.ReadAllText(inputFilePath);
                string pattern = @"\b[A-Z]{2}[0-9]{2}\s?[A-Z]{3}\b";
                Regex regex = new Regex(pattern);

                MatchCollection matches = regex.Matches(text);
                vehicleNumbers = new List<string>();
                foreach (System.Text.RegularExpressions.Match item in matches)
                {
                    vehicleNumbers.Add(item.Value);
                }
            }
            else
            {
                throw new FileNotFoundException("Input file not found.");
            }
        }

        

     

        [Then(@"the output should match expected results '([^']*)'")]
        public void ThenTheOutputShouldMatchExpectedResults(string expectedFilePath)
        {
            var ExpectedValuationDetails = ReadCsvFile(expectedFilePath);
            var ActualValuationDetails = valuationDetailsList;
            if(ActualValuationDetails.Count!= ExpectedValuationDetails.Count())
            {
                throw new Exception("Valuation details do not match");
            }
            for (int i = 0; i < ExpectedValuationDetails.Count; i++)
            {
                Assert.Equal(ExpectedValuationDetails[i].Manufacturer, ActualValuationDetails[i].Manufacturer);
                Assert.Equal(ExpectedValuationDetails[i].Model, ActualValuationDetails[i].Model);
                Assert.Equal(ExpectedValuationDetails[i].Year, ActualValuationDetails[i].Year);
                Assert.Equal(ExpectedValuationDetails[i].regNumber, ActualValuationDetails[i].regNumber);
            }
        }




    }
}
