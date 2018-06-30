using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    class Culture
    {
        public const string Ru = "ru-RU";
        public const string Eng = "en-US";
        public static CultureInfo EngInfo = CultureInfo.CreateSpecificCulture(Eng);
    }
}
