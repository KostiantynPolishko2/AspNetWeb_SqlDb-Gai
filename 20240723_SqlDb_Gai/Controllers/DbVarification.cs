using _20240723_SqlDb_Gai.Database;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace _20240723_SqlDb_Gai.Controllers
{
    public static class DbVarification
    {
        private static readonly string patternNumber = @"^[A-Z]{2}\d{4}[A-Z]{2}$";
        public static bool IsDbContext(CarContext carContext) => carContext.Database.CanConnect();

        public static bool IsDbCars(CarContext carContext) => carContext.Cars != null ? true : false;

        public static bool IsDbMarks(CarContext carContext) => carContext.Marks != null ? true : false;

        public static bool IsDbColors(CarContext carContext) => carContext.Colors != null ? true : false;

        public static float getPaintThk(float minThk, float maxThk) => (minThk + maxThk) / 2;

        public static bool isNumber(string number) => Regex.IsMatch(number, patternNumber, RegexOptions.IgnoreCase);
    }
}
