
namespace CakePrizeCore.libs.utils
{
    public static class StringUtils
    {
        public static string GetIngredientWithoutBrand(string text)
        {
            return text.Substring(0, text.IndexOf('-') - 1);
        }

        public static string GetBrandWithoutIngredient(string text)
        {
            return text.Substring(text.IndexOf('-') + 2);
        }
    }
}
