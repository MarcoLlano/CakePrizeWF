
using System;

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
            int packSize)
        {
            var ingredientPrizePerTotalAmount = GetProfitPercentage(GetWeightPercentage(ingredientAmount), costPrize) / packSize;
            return Math.Round(percentProfit / 100.0 * ingredientPrizePerTotalAmount + ingredientPrizePerTotalAmount, 2);
        }
    }
}
