
namespace CakePrize.libs.utils
{
    public static class UnitConvertion
    {
        public static double ConvertWeightVol(double weightVol, string unit)
        {
            switch (unit.Replace(" ", string.Empty))
            {
                case "mg":
                    return weightVol / 1000;
                case "ml":
                    return weightVol;
                case "lb":
                    return weightVol * 453.6;
                case "kg":
                    return weightVol * 1000;
                case "ctlla":
                    return weightVol * 2875;
                case "oz":
                    return weightVol * 28.35;
                case "g":
                    return weightVol;
                default:
                    return -1;
            }
        }

        public static float CalculateUnitPrice(float units, float packPrice)
        {
            return packPrice / units;
        }

        public static double PoundsToGrams(int v)
        {
            return v * 453.592;
        }
    }
}
