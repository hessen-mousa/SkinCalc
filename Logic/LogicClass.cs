using System;

namespace Logic
{
    public class LogicClass
    {
        /// <summary>
        /// Calculates how many game cases can be opened with available funds based on the costs of keys and cases.
        /// Validates inputs to ensure they are positive. Determines the total cost per case, the count of cases that
        /// can be opened, and the additional money needed for another case. Returns a CalculationLogicResponse with
        /// results or null for invalid inputs.
        /// </summary>
        /// <param name="availableMoney">Available money to spend.</param>
        /// <param name="keyCost">Cost of one key.</param>
        /// <param name="caseCost">Cost of one case.</param>
        /// <returns>CalculationLogicResponse with number of cases that can be opened and money needed for another case, or null for invalid inputs.</returns>
        public static CalculationLogicResponse CalcLogic(double availableMoney, double keyCost, double caseCost)
        {
            if (availableMoney <= 0 || keyCost <= 0 || caseCost <= 0) { return null; }

            // Calculate the total cost to open one case (key + case)
            double totalCostPerCase = keyCost + caseCost;
           
            // Calculate how many cases can be opened
            int numberOfCases = (int)Math.Floor((availableMoney / totalCostPerCase));

            double moneyForNextCase = totalCostPerCase - (availableMoney - numberOfCases * totalCostPerCase);
            return new() 
            {
                NumberOfCases = numberOfCases,
                MoneyForNextCase = moneyForNextCase,
            };

        }
    }
}
