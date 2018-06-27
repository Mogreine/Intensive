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

        public Converter()
        {

            _operationsDict = new SortedDictionary<string, string>
            {
                {"умножить на", "*"},
                {"делить на", "/"}
            };

        }

        public string ConvertTextToEquation(string str)
        {
            foreach (var pair in _operationsDict)
                str.Replace(pair.Key, pair.Value);

            return str;
        }
    }
}
