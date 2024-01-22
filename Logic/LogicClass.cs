using System;

namespace Logic
{
    public class LogicClass
    {
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
