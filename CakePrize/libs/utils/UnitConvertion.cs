using System;
using System.Collections.Generic;
using System.Text;

namespace CakePrize.libs.utils
{
    public static class UnitConvertion
    {
        public static double GramsToPounds(int v)
        {
            return v * 0.00220462;
        }

        public static double PoundsToGrams(int v)
        {
            return v * 453.592;
        }
    }
}
