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

        public static List<string> Steps(string expression)
        {
            var output = new List<string>();
            var parts = expression.Split(' ');
            double a;
            if (Double.TryParse(parts[0], out a))
            {
                for (var i = 1; i < parts.Length - 1; i += 2)
                {
                    var step = a + " " + parts[i] + " " + parts[i + 1] + " = ";
                    double b, c;
                    if (Double.TryParse(parts[i + 1], out b) && !Double.TryParse(parts[i], out c))
                    {
                        if (parts[i] == "+")
                            a += b;
                        else if (parts[i] == "-")
                            a -= b;
                        else if (parts[i] == "*")
                            a *= b;
                        else
                            a /= b;
                        step += a;
                        output.Add(step);
                    }
                    else
                    {
                        output.Clear();
                        output.Add("Выражение " + expression + " составлено неправильно.");
                        break;
                    }
                }
            }
            else
            {
                output.Add("Выражение " + expression + " составлено неправильно.");
            }

            if (output.Count == 0)
            {
                output.Add(a.ToString());
            }
            return output;
        }
    }
}
