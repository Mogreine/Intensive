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
        public static bool TryParse(string str, out double num)
        {
            return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out num);
        }
    }
}
