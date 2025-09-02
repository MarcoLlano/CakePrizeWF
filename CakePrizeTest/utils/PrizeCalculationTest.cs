using CakePrize.libs.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CakePrizeTest;

[TestClass]
public class PrizeCalculationTest
{
    [TestMethod]
    public void TestCalculateMilkMaxCostInBsFromMls()
    {
        double milkMls = 275;
        Assert.AreEqual(3.5062499999999996, PrizeCalculation.CalculateWeightVolCost(50, 160.0, 20, 50));
    }
}
