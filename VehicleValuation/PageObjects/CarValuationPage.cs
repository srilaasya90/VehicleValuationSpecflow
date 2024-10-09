using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using VehicleValuation.Models;
using SeleniumExtras.WaitHelpers;

namespace VehicleValuation.PageObjects
{
    public class CarValuationPage
    {

        private readonly IWebDriver _driver;

       
        public CarValuationPage(IWebDriver driver)
        {
            _driver = driver;
        }

       
        private string carRegNumber;
        private IWebElement acceptCookies => _driver.FindElement(By.Id("onetrust-accept-btn-handler"));
       
        private IWebElement MilageInput => _driver.FindElement(By.Id("Mileage"));
        private IWebElement SubmitButton => _driver.FindElement(By.Id("btn-go"));
      
        public void EnterRegistrationNumber(string regNumber)
        {
            carRegNumber = regNumber.Trim();
            acceptCookies.Click();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement RegistrationInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("vehicleReg")));

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
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            IWebElement milageInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("EmailAddress")));
            milageInput.Click();
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
            
            
            string milage = (string)jsExecutor.ExecuteScript("return document.getElementById('MileageCheck').value;");
             
            string Manufacturer = (string)jsExecutor.ExecuteScript("return document.evaluate(\"(//div[contains(text(),'Manufacturer')]/following-sibling::div)[2]\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent;");
            
            string Model = (string)jsExecutor.ExecuteScript("return document.evaluate(\"(//div[contains(text(),'Model')]/following-sibling::div)[2]\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent;");
            
            string year = (string)jsExecutor.ExecuteScript("return document.evaluate(\"(//div[contains(text(),'Year')]/following-sibling::div)[2]\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent;");

            ValuationDetails ValuationDetails = new ValuationDetails()
            {
                regNumber = carRegNumber,
                Manufacturer = Manufacturer,
                Model = Model,
                Year = year,
                Milage = milage
            };


            return ValuationDetails;

        }

    }
}
