using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using VehicleValuation.PageObjects;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Security.Cryptography;
using VehicleValuation.Models;
using FluentAssertions.Execution;
using System.Text;
using NPOI.SS.Formula.Functions;
using System.Formats.Asn1;
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
        public CarValuationStepDefinitions(IWebDriver driver, CarValuationPage valuationPage)
        {
            _driver = driver;
            _valuationPage = valuationPage;
        }

        [Given(@"user have a list of vehicle registration numbers from ""([^""]*)""")]
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

        [When(@"user perform car valuation using the valuation website")]
        public void WhenUserPerformCarValuationUsingTheValuationWebsite()
        {
            List<ValuationDetails> valuationDetailsList = new List<ValuationDetails>();
            foreach (string vehicleNumber in vehicleNumbers) {
                _driver.Navigate().GoToUrl("https://www.webuyanycar.com/");
                _valuationPage.EnterRegistrationNumber(vehicleNumber);
                _valuationPage.SubmitForm();
                ValuationDetails valuationDetails = _valuationPage.GetValuation();
                valuationDetailsList.Add(valuationDetails);
              
            }
            using (var writer = new StreamWriter(outputFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(valuationDetailsList);
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

        [Then(@"the output should match expected results for ""([^""]*)"" and ""([^""]*)""")]
        public void ThenTheOutputShouldMatchExpectedResultsForAnd(string expectedFilePath, string actualFilePath)
        {
            string[] outputData = File.ReadAllLines(outputFilePath);
            string[] expectedData = File.ReadAllLines(@"C:\Users\srila\OneDrive\Desktop\OutputFile.txt");

            for (int i = 0; i < expectedData.Length; i++)
            {
                if (outputData[i] != expectedData[i])
                {
                    throw new Exception($"Expected output file differs from Actual file at {i + 1}");
                }
            }

        }
    }
}
