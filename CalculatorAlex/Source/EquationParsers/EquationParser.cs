using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class EquationParser
    {

        public static List<string> Steps(string expression)
        {
            List<string> output = new List<string>();
            var parts = expression.Split(' ');
            double a;
            if (double.TryParse(parts[0], out a))
            {
                for (int i = 1; i < parts.Length - 1; i += 2)
                {
                    string step = a + " " + parts[i] + " " + parts[i + 1] + " = ";
                    double b;
                    if (double.TryParse(parts[i + 1], out b))
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
                        output.Add("Ошибка - " + parts[i + 1] + ".");
                        break;
                    }
                }
            }
            else
            {
                output.Add("Ошибка - " + parts[0] + ".");
            }
            return output;
        }
    }
}
