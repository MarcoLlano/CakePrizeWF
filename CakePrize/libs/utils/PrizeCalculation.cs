
using System;
using System.Text.RegularExpressions;

namespace CakePrize.libs.utils
{
    public static class PrizeCalculation
    {
        private static double GetWeightPercentage(double ingredientAmount)
        {
            return ingredientAmount / 1000 * 100;
        }

        private static double GetProfitPercentage(double percentage, double costPrize)
        {
            return percentage / 100 * costPrize;
        }

        public static double CalculateWeightVolCost(int percentProfit, double ingredientAmount, double costPrize, 
            int packSize, string unitType, int packQty)
        {
            if (unitType == "ud")
            {
                var cost = costPrize / packQty * ingredientAmount;
                return Math.Round(Math.Round(percentProfit / 100.0 * cost + cost, 2) / packSize, 2);
            }
            else
            {
                var ingredientPrizePerTotalAmount = GetProfitPercentage(GetWeightPercentage(ingredientAmount), costPrize) / packSize;
                return Math.Round(percentProfit / 100.0 * ingredientPrizePerTotalAmount + ingredientPrizePerTotalAmount, 2);
            }
        }
    }
}
