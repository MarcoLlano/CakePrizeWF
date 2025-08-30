
namespace CakePrize.libs.utils
{
    public static class PrizeCalculation
    {
        static int maxPercentageProfit = 100;
        static int minPercentageProfit = 50;

        public static double GetMaxFlourCostProfit(double flourGrams)
        {
            int flourPrizePerKg = 25;
            return CalculateWeightVolCost(maxPercentageProfit, flourGrams, flourPrizePerKg);
        }

        public static double GetMinFlourCostProfit(double flourGrams)
        {
            int flourPrizePerKg = 25;
            return CalculateWeightVolCost(minPercentageProfit, flourGrams, flourPrizePerKg);
        }

        public static double GetMaxMilkCostProfit(double milkMls)
        {
            double milkPrizeLt = 8.5;
            return CalculateWeightVolCost(maxPercentageProfit, milkMls, milkPrizeLt);
        }

        public static double GetMinMilkCostProfit(double milkMls)
        {
            double milkPrizeLt = 8.5;
            return CalculateWeightVolCost(minPercentageProfit, milkMls, milkPrizeLt);
        }

        public static double CalculateWeightVolCost(int percentProfit, double ingredientAmount, double costPrize)
        {
            var ingredientPrizePerTotalAmount = ingredientAmount * costPrize / 1000;
            return percentProfit / 100.0 * ingredientPrizePerTotalAmount + ingredientPrizePerTotalAmount;
        }
    }
}
