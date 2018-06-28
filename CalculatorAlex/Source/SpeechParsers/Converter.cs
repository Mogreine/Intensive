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
                str.Replace(pair.Key, pair.Value);
            return str;
        }
    }
}
