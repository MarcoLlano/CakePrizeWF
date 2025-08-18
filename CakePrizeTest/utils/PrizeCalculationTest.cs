using CakePrize.libs.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CakePrizeTest;

[TestClass]
public class PrizeCalculationTest
{
    [TestMethod]
    public void TestCalculateFlourMinCostInBsFromGrams()
    {
        double flourGrams = 275;
        Assert.AreEqual(10.3125, PrizeCalculation.GetMinFlourCostProfit(flourGrams));
    }

    [TestMethod]
    public void TestCalculateFlourMaxCostInBsFromGrams()
    {
        double flourGrams = 275;
        Assert.AreEqual(13.75, PrizeCalculation.GetMaxFlourCostProfit(flourGrams));
    }

    [TestMethod]
    public void TestCalculateMilkMinCostInBsFromMls()
    {
        double milkMls = 280;
        Assert.AreEqual(4.76, PrizeCalculation.GetMaxMilkCostProfit(milkMls));
    }

    [TestMethod]
    public void TestCalculateMilkMaxCostInBsFromMls()
    {
        double milkMls = 275;
        Assert.AreEqual(3.5062499999999996, PrizeCalculation.GetMinMilkCostProfit(milkMls));
    }
}
