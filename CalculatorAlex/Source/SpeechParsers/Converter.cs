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

        public Converter(string lang)
        {
            switch (lang)
            {
                case Culture.Ru:
                    _operationsDict = ReplaceableWords.OperationsRu;
                    _bigNumberDict = ReplaceableWords.BigNumbRu;
                    break;
                case Culture.Eng:
                    _operationsDict = ReplaceableWords.OperationsEn;
                    _bigNumberDict = ReplaceableWords.BigNumbEn;
                    break;
            }

        }

        private string TransformBigNumbers(string str)
        {
            var pieces = str.Split(' ').ToList();
            for (var i = 0; i < pieces.Count; ++i)
            {
                if (_bigNumberDict.ContainsKey(pieces[i]))
                {
                    var t = _bigNumberDict[pieces[i]];

                    double value;
                    if (i != 0 && DoubleParser.TryParse(pieces[i - 1], out value))
                    {
                        t *= value;
                        pieces[i] = t.ToString();
                        pieces.RemoveAt(i - 1);
                    }
                    else
                    {
                        pieces[i] = t.ToString();
                    }
                }
            }

            for (int i = 1; i < pieces.Count; ++i)
            {
                double value, value2;
                if (DoubleParser.TryParse(pieces[i - 1], out value) && DoubleParser.TryParse(pieces[i], out value2))
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

        public string PreConvertation(string str)
        {
            str = str.ToLower();
            foreach (var pair in _operationsDict)
                str = str.Replace(pair.Key, pair.Value);
            str = TransformBigNumbers(str);
            return str;
        }

        public string ConvertTextToEquation(string str)
        {
            var symbols = str.ToCharArray();
            var flag = false;
            for (var i = 0; i < symbols.Length; ++i)
            {
                if (symbols[i] == ' ')
                    continue;
                if (Char.IsDigit(symbols[i]))
                    flag = false;
                else if (flag || i == 0)
                {
                    if (i + 1 < symbols.Length && symbols[i + 1] == ' ')
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
