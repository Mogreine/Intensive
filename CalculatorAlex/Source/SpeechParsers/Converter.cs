using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class Converter
    {
        private readonly SortedDictionary<string, string> _operationsDict;
        private readonly SortedDictionary<string, string> _numberDict;

        public Converter()
        {

            _operationsDict = new SortedDictionary<string, string>
            {
                {"умножить на", "*"},
                {"делить на", "/"},
                {"запятая", ","},
                {"точка", "."}
            };

            _numberDict = new SortedDictionary<string, string>
            {
                {"сто", "100"},
                {"тысяча", "1000"},
                {"миллион", "1000000"}
            };
        }

        private string TransformNumbers(string str)
        {
            foreach (var pair in _numberDict)
                str.Replace(pair.Key, pair.Value);
            /*
            string ans = "";
            string num1 = "", num2 = "";
            bool flag = false;
            for (int i = 0; i < str.Length; ++i)
            {
                if (Char.IsDigit(str[i]))
                {
                    if (num1)
                    num1 += str[i];
                }
            }
            */
            return str;
        }

        public string ConvertTextToEquation(string str)
        {
            str = str.ToLower();
            foreach (var pair in _operationsDict)
                str.Replace(pair.Key, pair.Value);
            str = TransformNumbers(str);
            return str;
        }
    }
}
