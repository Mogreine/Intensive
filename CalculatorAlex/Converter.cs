using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    class Converter
    {
        private static SortedDictionary<string, char> repOperators = new SortedDictionary<string, char>
        {
            {"умножить на", '+'},
            {"делить на", '-'}
        };
        public static string ConvertString(string str)
        {
            foreach (var pair in repOperators)
                str.Replace(pair.Key, pair.Value.ToString());
            return str;
        }
    }
}
