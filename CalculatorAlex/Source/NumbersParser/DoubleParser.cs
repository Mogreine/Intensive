using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    class DoubleParser
    {
        public static CultureInfo EngInfo = CultureInfo.CreateSpecificCulture("en-US");

        public static bool TryParse(string str, out double num)
        {
            return double.TryParse(str, NumberStyles.Any, EngInfo, out num); // CultureInfo.InvariantCulture
        }
    }
}
