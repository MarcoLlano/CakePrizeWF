using CakePrize.libs.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CakePrizeTest.utils
{
    [TestClass]
    public class UnitconvertionTest
    {
        [TestMethod]
        public void TestPoundsToGrams()
        {
            Assert.AreEqual(2267.96, UnitConvertion.PoundsToGrams(5));
        }

        [TestMethod]
        public void TestGramsToPounds()
        {
            Assert.AreEqual(0.110231, UnitConvertion.GramsToPounds(50));
        }
    }
}