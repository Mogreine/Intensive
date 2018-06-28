using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class Converter
    {
        private readonly Dictionary<string, string> _operationsDict;

        public Converter()
        {

            _operationsDict = new Dictionary<string, string>
            {
                {"умножить на", "*"},
                {"делить на", "/"},
                {"запятая", "."},
                {"точка", "."},
                {"умножить", "*"},
                {"делить", "/"},
                {"плюс", "+"},
                {"минус", "-"},
                {"minus", "-"},
                {"plus", "+"}
            };

        }

        public string ConvertTextToEquation(string str)
        {
            str = str.ToLower();
            foreach (var pair in _operationsDict)
                str = str.Replace(pair.Key, pair.Value);
            char[] symbols = str.ToCharArray();
            bool flag = false;
            for (int i = 0; i < symbols.Length; ++i)
            {
                if (symbols[i] == ' ')
                    continue;
                if (Char.IsDigit(symbols[i]))
                    flag = false;
                else if (flag || i == 0)
                {
                    if (symbols[i + 1] == ' ')
                    {
                        symbols[i + 1] = symbols[i];
                        symbols[i] = ' ';
                    }
                    flag = false;
                }
                else
                    flag = true;

            }
            str = new string(symbols);
            str = str.Trim(' ');
            str = str.Replace("  ", " ");
            return str;
        }
    }
}
