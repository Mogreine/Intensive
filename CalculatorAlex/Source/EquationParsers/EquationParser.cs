using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class EquationParser
    {

        public static bool haveProblem;
        public static double lastValue;

        public static List<string> Steps(string expression)
        {
            var output = new List<string>();
            var parts = expression.Split(' ');
            double res;
            if (DoubleParser.TryParse(parts[0], out res))
            {
                for (var i = 1; i < parts.Length - 1; i += 2)
                {
                    var step = res + " " + parts[i] + " " + parts[i + 1] + " = ";
                    double nextOperand;
                    if (DoubleParser.TryParse(parts[i + 1], out nextOperand))
                    {
                        if (parts[i] == "+")
                            res += nextOperand;
                        else if (parts[i] == "-")
                            res -= nextOperand;
                        else if (parts[i] == "*")
                            res *= nextOperand;
                        else
                        {
                            res /= nextOperand;
                            res = Math.Round(res, 3);
                        }
                        step += res;
                        output.Add(step);
                    }
                    else
                    {
                        haveProblem = true;
                        lastValue = res;
                        output.Add("Выражение " + expression + " составлено неправильно.");
                        break;
                    }
                }
            }
            else
            {
                haveProblem = true;
                lastValue = res;
                output.Add("Выражение " + expression + " составлено неправильно.");
            }

            if (output.Count == 0)
            {
                output.Add(res.ToString(CultureInfo.InvariantCulture));
            }
            return output;
        }
    }
}
