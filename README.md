# Compound-Interest

Tasks:
1. Write a regression test suite in MS Test framework (multiple tests) for testing this website → "http://www.egalegal.com/compoundWindow.html"
2. Each test should do the following things: 
i. Calculate compound interest using this website and write the answer given by website into a log file.
ii. Calculate compound interest on your own (inside the test, using code and math formulae).
iii. Now compare and assert the answer given by website, which is written in log files, by matching it with your calculated value.
3. Write multiple tests, with different parameters (different rate of interest, different principal amount, etc) and assert the answer provided by the website with your calculated answer.

Workflow:

a. Fields
1. driver - creating object of ChromeDriver class
2. result - store the result calculated by website
3. string path_to_file - path of log file
4. result_Calculated - store the result calculated by the function
5. accrued_Calculated – store the accrued calculated by the function
6. amount_to_test = amount ( that is to be used in test case)
7. values – stores the values read from the log file

b. Methods
1. Format  – to return the string ( result, accrued) in proper format
2. CalculateCompoundInterest -  calculates the CI based on the inputs provided
3. Send_Amount – extracts the element with “amount” id and enters the amount on the website
4. Send_Interest - extracts the element with “interest” id and enters the interest on the website
5. Send_Amount_And_Interest – do both the above steps
6. WriteToFile - write the result calculated by website in the log file
7. ReadLogFile - to extract data from "log.txt", store into variables and print it
8. Validation – for asserts and checks 
9. Result – returns the result from the website as a string (e.g. result + “ “ + accrued) 
10. Handler – includes the common functions of almost all the methods
11. Validator – for validation and calculating accrued_Calculated ( accrued calculated by the code)
12. Selector_Of_Date_CheckBox_Strategy – selects the dropdown’s options and checkbox (if checkbox is passed as true)
13. Alert_Hander – on selecting start date  > end date, the website shows up alert, this function handles that and set the start values again to calculate values

c. TestInitilize
1. Opens browser
2. Maximizes the window
3. Sets the implicit wait
4. Go to the website Url

d. Test Methods
1. Amount_Check_Interest , Check - to check the working of methods (Send_Amount_And_Interest, Send_Amount, Send_Interest, CalculateCompountInterest)
2. TestMethod1 - no field is entered and directly Calculate is clicked, get the result from the website, clear the content of the log file, write the result calculated into the "log.txt" in form of string, read from the log file, calculate CI using function, then asserts and checks are performed
3. TestMethod2 - amount entered is lesser than interest rate, checked the checkbox, all dropdowns are also selected and then Calculate is clicked, get the result from the website
, clear the content of the log file, write the result calculated into the "log.txt" in form of string, read from the log file, calculate CI using function, then asserts and checks are performed
4. TestMethod3 - only amount and interest are entered and then Calculate button is clicked (after that same steps as above)
5. TestMethod4 - amount and interest are entered, checks the checkbox, selects from dropdown and then Calculate button is clicked (after that same steps as above)
6. TestMethod5 - different dropdown option is chosen with respect to TestMethod4 ( rest everything is same)
7. TestMethod6- different dropdown option is chosen with respect to TestMethod4 and TestMethod5 ( rest everything is same)
8. TestMethod7 - changed the interest to 10 and amount is changed to 200, amount and interest are entered, checks the checkbox, selects from dropdown and then Calculate button is clicked (after that same steps as in point 3))
9. TestMethod8 - only amount (negative value) and interest (negative value) are entered and then Calculate button is clicked (after that same steps as above)
10. TestMethod9 - only amount and interest are entered, here interest entered is "1" and then Calculate button is clicked (after that same steps as above)
11. TestMethod10 - handles the case when start date is greater than end date and alert shows up

e. TestCleanup
1. Closes the browser
2. Quits 

