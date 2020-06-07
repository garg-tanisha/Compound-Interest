using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Collections;

namespace Compound_Interest
{ 

    [TestClass]
    public class CI
    {
    	#region Fields

        // creating object of ChromeDriver class
        IWebDriver driver = new ChromeDriver();

        //store the result calculated by website
        string result;

        //path of log file
        string path_to_file = @"C:\Users\YOGESH GARG\source\repos\Compound Interest\Compound Interest\log.txt";

        //store the result calculated by the function 
        string result_Calculated;

        //store the result calculated by the function
        string accrued_Calculated;

        //amount ( that is to be used in test case)
        double amount_to_test = 0;

        // IList values = ["0.00", "0.00"]; //error
        IList values;

        #endregion

        #region Methods

        //to format the result calculated
        string Format(double result)
        {
            string resultString = (result * 100).ToString();

            string dot = ".";

            //find index of dot string in resultString
            int decPt = resultString.IndexOf(dot);

            if (decPt != -1)
            {
                //Rounds a double-precision floating-point value to the nearest integral value, and rounds midpoint values to the nearest even number
                resultString = ((Math.Round(Convert.ToDouble(resultString))) / 100).ToString();

                decPt = resultString.IndexOf(dot);

                //if after dot there is only one digit
                if (decPt == resultString.Length - 2)
                {
                    //concatenation
                    resultString = resultString + "0";

                    return resultString;
                }
            }
            else
            {
                //type conversion from double to string
                resultString = (result).ToString();

                decPt = resultString.IndexOf(dot);

                if (decPt == -1)
                {
                    //concatenation
                    resultString = result + ".00";

                    return resultString;
                }
                else if (decPt == resultString.Length - 1)
                {
                    //concatenation
                    resultString = result + "0";

                    return resultString;
                }
            }

            return resultString;
        }

        // to calculate Compoound Interest
        string CalulateCompoundInterest(double amount = 0.0, double interest = 0.0, int rests = 1, int endYear = 0, int endMonth = 0, int endDate = 0, int startYear = 0, int startMonth = 0, int startDate = 0, bool checkbox = false)
        {
            int yearLength = 365;
            
            //if checkbox is checked
            if (checkbox)
            {
                yearLength = 360;
            }

            //calculate absolute value
            amount = Math.Abs(amount);

            //calculate absolute value
            interest = Math.Abs(interest);

            //initilizes a new instance of the DateTime structure to the specified year, month, day
            DateTime sDate = new DateTime(startYear + 1985, startMonth + 1, startDate + 1);

            DateTime eDate = new DateTime(endYear + 1985, endMonth + 1, endDate + 1);

            //TimeSpan.Days - Gets the days component of the time interval represented by the current time span structure
            int termDays = (eDate - sDate).Days;

            //formulaes
            double exponensh = (termDays * rests) / (double)yearLength;

            double firstBit = 1 + interest / (rests * 100);

            //result
            double result = Math.Pow(firstBit, exponensh) * amount;

            //to get a proper format
            string res = Format(result);

            return res;
        }

        //Enter the amount in the website
        void Send_Amount(string amount)
        {

            //finds the Element with mentioned Id, and fill amount's value there
            if (!amount.Equals(null))
                driver.FindElement(By.Id("amount")).SendKeys(amount);
            
        }

        //Enter the interest rate in the website
        void Send_Interest(string interest)
        {

             //finds the Element with mentioned Id, and fill interest's value there
            if (!interest.Equals(null))
               driver.FindElement(By.Id("interest")).SendKeys(interest);
         
        }

        //Enter the amount and the interest rate in the website
        void Send_Amount_And_Interest(string amount, string interest)
        {
        	Send_Amount(amount);

            Send_Interest(interest);
        }

        //write the result calculated by website in the log file
        public static void WriteToFile(String value, String filename)
        {
            //opening txt file
            var file = new FileStream(filename, FileMode.OpenOrCreate);

            var sw = new StreamWriter(file);

            //writing to the file
            sw.WriteLine(value);

            //closing file
            sw.Close();
        }

        //to extract data from "log.txt", store into variables and print it
        public void ReadLogFile()
        {
            //System.IO.SystemReader implements a System.IO.TextReader that reads a character from a byte stream in particular encoding
            System.IO.StreamReader reader = new System.IO.StreamReader(path_to_file);

            //gets the value that indicates whether the current stream position is at the end of the stream
            while (!reader.EndOfStream)
            {

                //reads line of characters from current stream and returns data as a string
                var line = reader.ReadLine();

                //splits the line read with respect to ' '
                values = line.Split(' ');

            }
        }

        //for assert and checks
        void Validation(string result, string accrued, string result_Calculated, string accrued_Calculated)
        {
            //results
            Console.WriteLine("Result : " + result);

            Console.WriteLine("Result_Calculated : " + result_Calculated);

            Console.WriteLine("Accrued : " + accrued);

            Console.WriteLine("Accrued_Calculator : " + accrued_Calculated);

            //checks
            Console.WriteLine(result_Calculated == result);

            Console.WriteLine(accrued_Calculated == accrued);

            //Asserts
            Assert.IsTrue(result_Calculated == result);

            Assert.IsTrue(accrued_Calculated == accrued);
        }

        //result from the website is returned here
        string Result()
        {
            //get the values of the elements with the result and accrued id's 
           return (driver.FindElement(By.Id("result")).GetAttribute("value") + " " + driver.FindElement(By.Id("accrued")).GetAttribute("value"));
        }

        //some functions
        void Handler()
        {
            //click the "Calculate" button
            driver.FindElement(By.Name("submit")).Click();

            //get the result from the website
            result = Result();

            //clear the content of the log file
            File.WriteAllText(path_to_file, String.Empty);

            //write the result calculated into the "log.txt" in form of string
            WriteToFile(result, path_to_file);

            //read from the log file 
            ReadLogFile();
        }

        //for validation
        void Validator()
        { 
            //calculate accrued Calculated by the function and calling Format() to get the proper format
            accrued_Calculated = Format(Convert.ToDouble(result_Calculated) - Math.Abs(amount_to_test));

            //for Asserts and checks
            Validation(values[0].ToString(), values[1].ToString(), result_Calculated, accrued_Calculated);
        }

        //for selecting, checking
        void Selector_Of_Date_CheckBox_Strategy(int rests = 1, int endYear = 0, int endMonth = 0, int endDate = 0, int startYear = 0, int startMonth = 0, int startDate = 0, bool checkbox = false)
        {
            //select start date
            IWebElement selectElement = driver.FindElement(By.Id("startDay"));

            var selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(startDate);

            //select startmonth
            selectElement = driver.FindElement(By.Id("startMonth"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(startMonth);

            // Select start year
            selectElement = driver.FindElement(By.Id("startYear"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(startYear);

            //select end date
            selectElement = driver.FindElement(By.Id("endDay"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(endDate);

            //select end month
            selectElement = driver.FindElement(By.Id("endMonth"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(endMonth);

            //select end year
            selectElement = driver.FindElement(By.Id("endYear"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(endYear);

            //select no of rests each year
            selectElement = driver.FindElement(By.Id("rests"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(rests);

            //if checkbox is true, check the checkbox
            if (checkbox)
                driver.FindElement(By.Id("shortYear")).Click();
        }

        //a alert is raised and handled because the start date is entered is larger than end date. After that, start date is entered such that it is smaller with respect to end date
        void Alert_Handler()
        {
            IWebElement selectElement = driver.FindElement(By.Id("endDay"));

            var selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(1);

            //select end month
            selectElement = driver.FindElement(By.Id("endMonth"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(1);

            //select end year
            selectElement = driver.FindElement(By.Id("endYear"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(1);

            //select start date
            selectElement = driver.FindElement(By.Id("startDay"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(2);

            //select startmonth
            selectElement = driver.FindElement(By.Id("startMonth"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(2);

            // Select start year
            selectElement = driver.FindElement(By.Id("startYear"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(2);

            //click Calculate
            driver.FindElement(By.Name("submit")).Click();

            //Explicit wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Wait for the alert to be displayed and store it in a variable
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

            //Store the alert text in a variable
            string text = alert.Text;

            //Press the OK button
            alert.Accept();

            //select start date
            selectElement = driver.FindElement(By.Id("startDay"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(1);

            //select startmonth
            selectElement = driver.FindElement(By.Id("startMonth"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(1);

            // Select start year
            selectElement = driver.FindElement(By.Id("startYear"));

            selectObject = new SelectElement(selectElement);

            // Select an <option> based upon the <select> element's internal index
            selectObject.SelectByIndex(0);

        }

        #endregion

 		[TestInitialize]
        public void Initilize()
        {
            //maximize the browser
            driver.Manage().Window.Maximize();

            //set Implicit Time Wait as well
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //set url of driver 
            driver.Url = "http://www.egalegal.com/compoundWindow.html";

        }

        #region TestMethods
        // to check the working of method
        //[TestMethod]
        public void Amount_Check_Interest()
        {
       
            Send_Amount_And_Interest("100", ""); // works fine

            //click on "Calculate" button
            driver.FindElement(By.Name("submit")).Click();

            Send_Interest("1");

            Send_Amount("100");

        }

        //to check the working of the function written to calculate CI
        //[TestMethod]
        public void Check()
        {
            try
            {
                //calculation
                string res = CalulateCompoundInterest(100, 2, 12, 6, 5, 5, 5, 5, 5, true);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "102.05");

                //calculation
                res = CalulateCompoundInterest(100, 2, 2, 6, 5, 5, 5, 5, 5, true);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "102.04");

                //calculation
                res = CalulateCompoundInterest(100, 2, 1, 6, 5, 5, 5, 5, 5, true);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "102.03");

                //calculation
                res = CalulateCompoundInterest();

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "0.00");

                //calculation
                res = CalulateCompoundInterest(-100, -1);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "100.00");

                //calculation
                res = CalulateCompoundInterest(100, 1);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "100.00");
               
                //calculation
                res = CalulateCompoundInterest(100, 1, 1, 1, 1, 1, 0, 1, 1);

                Console.WriteLine(res);

                //checks
                Console.WriteLine(res == "101.00");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        //no field is entered and directly Calculate is clicked
        //0.0 0.0
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                amount_to_test = 0;

                //included muultiple fucntions
                Handler();

                //calculate CI using function
                result_Calculated = CalulateCompoundInterest();

                //for checks and asserts
                Validator();
            }
            catch (Exception e)
            {
                //prints details about the exception encountered
                Console.WriteLine(e);
            }
        }
        
        //amount enetered is lesser than interest rate, checked the checkbox, all dropdowns are also selected
        //1935.01 1735.01
        [TestMethod]
        public void TestMethod2()
        {
            try
            {
                //enter amount and interest in website
                Send_Amount_And_Interest("200", "300");

                //selects dropdowns and check checkbox
                Selector_Of_Date_CheckBox_Strategy(2, 6, 5, 5, 5, 5, 5, true);

                amount_to_test = 200;

                //have multiple functions
                Handler();

                //calculate CI using function
                result_Calculated = CalulateCompoundInterest(amount_to_test, 300, 4, 6, 5, 5, 5, 5, 5, true);

                //for checks and asserts
                Validator();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //only amount and interest are entered and then Calculate button is clicked
        //100.00 0.00
        [TestMethod]
        public void TestMethod3()
        {
            try
            {
                Send_Amount_And_Interest("100", "");

                amount_to_test = 100;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test);

                Validator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //amount and interest are entered, checkes the checkbx, selects from dropdown
        //102.05 2.05
        [TestMethod]
        public void TestMethod4()
        {
            try
            {
                Send_Amount_And_Interest("100", "2");

                Selector_Of_Date_CheckBox_Strategy(3,6,5,5,5,5,5,true);

                amount_to_test = 100;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test,2,12,6,5,5,5,5,5,true);

                Validator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //different dropdown option is choosen 
        //102.04 2.04

        [TestMethod]
        public void TestMethod5()
        {
            try
            {
                Send_Amount_And_Interest("100", "2");

                Selector_Of_Date_CheckBox_Strategy(1,6,5,5,5,5,5,true);

                amount_to_test = 100;

                Handler();

                // string res = CalulateCompoundInterest(100, 2, 12, 6, 5, 5, 5, 5, 5, true);

                result_Calculated = CalulateCompoundInterest(amount_to_test, 2, 2, 6, 5, 5, 5, 5, 5, true);

                Validator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //different dropdown option is chosnen
        //102.03 2.03
        [TestMethod]
        public void TestMethod6()
        {
            try
            {
                Send_Amount_And_Interest("100", "2");

                Selector_Of_Date_CheckBox_Strategy(0, 6, 5, 5, 5, 5, 5, true);

                amount_to_test = 100;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test,2,1,6,5,5,5,5,5,true);

                Validator();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //changed the interest to 10 
        //221.07 21.07
        [TestMethod]
        public void TestMethod7()
        {
            try
            {
                Send_Amount_And_Interest("200", "10");

                Selector_Of_Date_CheckBox_Strategy(2, 6, 5, 5, 5, 5, 5, true);

                amount_to_test = 200;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test, 10, 4, 6, 5, 5, 5, 5, 5, true);

                Validator();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //negative values entered
        //100.00 0.00
        [TestMethod]
        public void TestMethod8()
        {
            try
            {
                Send_Amount_And_Interest("-100","-1");

                amount_to_test = -100;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test, -1);

                Validator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //only amount and interest are entered, here interest entered is "1"
        //100.00 0.00
        [TestMethod]
        public void TestMethod9()
        {
            try
            {
                Send_Amount_And_Interest("100", "1");

                amount_to_test = 100;

                Handler();

                result_Calculated = CalulateCompoundInterest(amount_to_test, 1);

                Validator();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        //handles the case when start date is greater than end date and alert shows up
        //101.00 1.00
        [TestMethod]
        public void TestMethod10()
        {
            try
            {
                Send_Amount_And_Interest("100", "1");

                //handles the case when start date is greater than end date and alert shows up
                Alert_Handler();

                amount_to_test = 100;

                Handler();

                //nothing provided for checkbox so it will remain unchecked
                result_Calculated = CalulateCompoundInterest(amount_to_test, 1, 1,1,1,1,0,1,1);

                Validator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        [TestCleanup]
        public void CleanUp()
        {
            //closes the browser
            driver.Close();

            //most important step
            driver.Quit();
        }
    }
}
