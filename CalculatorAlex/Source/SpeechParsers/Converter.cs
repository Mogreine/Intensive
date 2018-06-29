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
        private readonly Dictionary<string, double> _bigNumberDict;

        public Converter()
        {

            _operationsDict = new Dictionary<string, string>
            {
                {"плюс-минус ", "+ -" },
                {"умножить на", "*"},
                {"делить на", "/"},
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

            _bigNumberDict = new Dictionary<string, double>
            {
                {"млн", 1000000},
                {"млрд", 1000000000}
            };
        }
        private string TransformBigNumbers(string str)
        {
            var pieces = str.Split(' ').ToList();
            for (int i = 0; i < pieces.Count; ++i)
            {
                if (_bigNumberDict.ContainsKey(pieces[i]))
                {
                    double t = _bigNumberDict[pieces[i]];

                    double value = 0;
                    if (i != 0 && Double.TryParse(pieces[i - 1], out value))
                    {
                        t *= value;
                        pieces[i] = t.ToString();
                        pieces.RemoveAt(i - 1);
                    }
                }
            }

            for (int i = 1; i < pieces.Count; ++i)
            {
                double value = 0, value2 = 0;
                if (Double.TryParse(pieces[i - 1], out value) && Double.TryParse(pieces[i], out value2))
                {
                    value2 += value;
                    pieces[i] = value2.ToString();
                    pieces[i - 1] = "";
                }
            }

            var sb = new StringBuilder();
            foreach (var p in pieces)
            {
                if (p == "")
                    continue;
                sb.Append(p + " ");
            }
            return sb.ToString().Trim(' ');
        }
        public string ConvertTextToEquation(string str)
        {
            str = str.ToLower();
            foreach (var pair in _operationsDict)
                str = str.Replace(pair.Key, pair.Value);
            str = TransformBigNumbers(str);
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
