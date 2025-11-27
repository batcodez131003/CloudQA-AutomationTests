using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CloudQA.AutomationTests
{
    [TestFixture]
    public class AutomationPracticeFormTests
    {
        private IWebDriver? _driver;   
        private const string BaseUrl = "https://app.cloudqa.io/home/AutomationPracticeForm";

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
           

            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _driver.Navigate().GoToUrl(BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();   
            }
        }

    
        private IWebElement FindInputByLabel(string labelText)
        {
            if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");

            string xpath = $"//*[normalize-space()='{labelText}']//following::input[1]";
            return _driver.FindElement(By.XPath(xpath));
        }

        private void SetTextField(string labelText, string value)
        {
            var input = FindInputByLabel(labelText);
            input.Clear();
            input.SendKeys(value);
        }

        private IWebElement FindRadioByOptionText(string optionText)
        {
            if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");

            var radioLocator = By.XPath(
                "//label[normalize-space()='" + optionText + "']//input" +
                " | " +
                "//*[normalize-space()='" + optionText + "']/preceding-sibling::input[1]"
            );

            return _driver.FindElement(radioLocator);
        }


        private IWebElement FindSelectByLabel(string labelText)
        {
            if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");

            string xpath = $"//*[normalize-space()='{labelText}']//following::select[1]";
            return _driver.FindElement(By.XPath(xpath));
        }

        // ==================================================================
        // TEST 1: First Name field accepts and retains entered text
        // ==================================================================
        [Test]
        public void FirstNameField_AllowsEnteringText()
        {
            const string expectedFirstName = "John";

            SetTextField("First Name", expectedFirstName);

            var firstNameInput = FindInputByLabel("First Name");
            string actualValue = firstNameInput.GetAttribute("value");

        
            if (actualValue != expectedFirstName)
            {
                throw new Exception(
                    $"First Name input value was '{actualValue}', expected '{expectedFirstName}'.");
            }
        }

        // ==================================================================
        // TEST 2: Gender – user can select 'Male' option
        // ==================================================================
        [Test]
        public void Gender_MaleOptionCanBeSelected()
        {
            var maleRadio = FindRadioByOptionText("Male");

            maleRadio.Click();

            
            if (!maleRadio.Selected)
            {
                throw new Exception("Male gender radio button was not selected after clicking.");
            }
        }

        // ==================================================================
        // TEST 3: Country dropdown – user can select 'India'
        // ==================================================================
        [Test]
        public void CountryDropdown_AllowsSelectingIndia()
        {
            var countrySelectElement = FindSelectByLabel("Country");

            var select = new OpenQA.Selenium.Support.UI.SelectElement(countrySelectElement);
            select.SelectByText("India");

            string selectedText = select.SelectedOption.Text;

          
            if (selectedText != "India")
            {
                throw new Exception(
                    $"Country dropdown selected value was '{selectedText}', expected 'India'.");
            }
        }

    }
}
