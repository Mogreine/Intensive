using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    class ReplaceableWords
    {
        public static Dictionary<string, string> OperationsRU = new Dictionary<string, string>
            {
                {"x", "*" },
                {"х", "*" },
                {"плюс-минус ", "+ -" },
                {"умножить на", "*"},
                {"разделить на", "/"},
                {"делить на", "/"},
                {"поделить на", "/"},
                {"запятая", "."},
                {"точка", "."},
                {"умножить", "*"},
                {"делить", "/"},
                {"плюс", "+"},
                {"минус", "-"},
                {"minus", "-"},
                {"plus", "+"},
                {",", "."}
            };

        public static Dictionary<string, double> BigNumbRU = new Dictionary<string, double>
            {
                {"один", 1},
                {"млн", 1000000},
                {"миллионов", 1000000},
                {"миллион", 1000000},
                {"миллиона", 1000000},
                {"миллиард", 1000000000},
                {"миллиарда", 1000000000},
                {"миллиардов", 1000000000},
                {"млрд", 1000000000}
            };

        public static Dictionary<string, string> OperationsEN = new Dictionary<string, string>
            {
                {"x", "*" },
                {"х", "*" },
                {"free", "3" },
                {"minus", "-"},
                {"plus", "+"},
                {"divided", "/"},
                {"divide", "/"},
                {"multiply", "*"},
                {",", "."}
            };

        public static Dictionary<string, double> BigNumbEN = new Dictionary<string, double>
            {
                {"million", 1000000},
                {"billion", 1000000000}
            };
    }
}
