using Logic;
using NUnit.Framework;

namespace NUnitTests

{
    public class LogicTests
    {
        
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        [TestCase(30.75, 1, 2, 10, 2.25)] // Valid input
        [TestCase(15.99, 0.5, 1, 10, 0.51)] // Valid input
        [TestCase(20.50, 2, 3, 4, 4.50)] // Valid input
        // ... additional valid test cases
        public void CalcLogic_SuccessTest(double availableMoney, double keyCost, double caseCost, int expectedNumberOfCases, double expectedMoneyForNextCase)
        {
            CalculationLogicResponse actualResponse = LogicClass.CalcLogic(availableMoney, keyCost, caseCost);

            Assert.IsNotNull(actualResponse, "Response should not be null.");
            Assert.That(actualResponse.NumberOfCases, Is.EqualTo(expectedNumberOfCases), $"Number of cases should be {expectedNumberOfCases}.");
            Assert.That(actualResponse.MoneyForNextCase, Is.EqualTo(expectedMoneyForNextCase).Within(0.01), $"Money for next case should be approximately {expectedMoneyForNextCase}.");
        }



        [Test]
        [TestCase(-50.25, 5, 5)] // Negative available money
        [TestCase(50.25, -5, 5)] // Negative key cost
        [TestCase(50.25, 5, -5)] // Negative case cost
        [TestCase(0, 0, 0)] // All values zero
        // ... additional invalid test cases
        public void CalcLogic_FailTest(double availableMoney, double keyCost, double caseCost)
        {
            CalculationLogicResponse actualResponse = LogicClass.CalcLogic(availableMoney, keyCost, caseCost);

            Assert.IsNull(actualResponse, "Response should be null for invalid inputs.");
        }
    }
}