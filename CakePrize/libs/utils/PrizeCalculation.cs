
using System;

namespace CakePrize.libs.utils
{
    public static class PrizeCalculation
    {
        public static double CalculateWeightVolCost(int percentProfit, double ingredientAmount, double costPrize, int productTotalSize)
        {
            productTotalSize = productTotalSize == 0 ? 1000 : productTotalSize;
            var ingredientPrizePerTotalAmount = ingredientAmount * costPrize / productTotalSize;
            return Math.Round(percentProfit / 100.0 * ingredientPrizePerTotalAmount + ingredientPrizePerTotalAmount, 2);
        }
    }
}
