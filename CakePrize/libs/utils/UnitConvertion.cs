using CakePrizeCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakePrize.libs.utils
{
    public static class UnitConvertion
    {
        public static double ConvertWeightToGram(double weight, string unit)
        {
            switch (unit.Replace(" ", string.Empty))
            {
                case "mg":
                    return weight / 1000;
                case "lb":
                    return weight * 453.6;
                case "kg":
                    return weight * 1000;
                case "ctlla":
                    return weight * 2875;
                case "oz":
                    return weight * 28.35;
                case "g":
                    return weight;
                default:
                    return -1;
            }
        }

        public static double PoundsToGrams(int v)
        {
            return v * 453.592;
        }
    }
}
