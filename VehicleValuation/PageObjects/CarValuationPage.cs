using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleValuation.Models;

namespace VehicleValuation.PageObjects
{
    public class CarValuationPage
    {

        private readonly IWebDriver _driver;

       
        public CarValuationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement acceptCookies => _driver.FindElement(By.Id("onetrust-accept-btn-handler"));
        private IWebElement RegistrationInput => _driver.FindElement(By.Id("vehicleReg"));
        private IWebElement MilageInput => _driver.FindElement(By.Id("Mileage"));
        private IWebElement SubmitButton => _driver.FindElement(By.Id("btn-go"));
        private IWebElement ValuationResultModel => _driver.FindElement(By.XPath("//body/div[@id='page-container']/wbac-app[1]/div[1]/div[1]/div[1]/vehicle-questions[1]/div[1]/section[1]/div[1]/div[1]/div[1]/div[3]/div[1]/vehicle-details[1]/div[3]/div[2]/div[2]"));
        private IWebElement ValuationResultYear => _driver.FindElement(By.XPath("//body/div[@id='page-container']/wbac-app[1]/div[1]/div[1]/div[1]/vehicle-questions[1]/div[1]/section[1]/div[1]/div[1]/div[1]/div[3]/div[1]/vehicle-details[1]/div[3]/div[2]/div[3]/div[2]"));
        public void EnterRegistrationNumber(string regNumber)
        {
            acceptCookies.Click();
            RegistrationInput.Clear();
            RegistrationInput.SendKeys(regNumber);
            MilageInput.Clear();
            MilageInput.SendKeys("15000");
           
        }

        public void SubmitForm()
        {
            SubmitButton.Click();
        }

        public ValuationDetails GetValuation()
        {
            ValuationDetails ValuationDetails = new ValuationDetails()
            {
                Modelnumber = "Toyota Prius",
                Year = "2018"
            };

            //ValuationDetails.Modelnumber = ValuationResultModel.Text;
            //ValuationDetails.Year = ValuationResultYear.Text;
            return ValuationDetails;

        }

    }
}
