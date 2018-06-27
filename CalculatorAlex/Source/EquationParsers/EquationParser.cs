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
            double a = double.Parse(parts[0]);
            for (int i = 1; i < parts.Length; i++)
            {
                string step = a + " " + parts[i][0] + " " + parts[i].Substring(1) + " = ";
                double b;
                if (double.TryParse(parts[i].Substring(1), out b))
                {
                    if (parts[i][0] == '+')
                        a += b;
                    else if (parts[i][0] == '-')
                        a -= b;
                    else if (parts[i][0] == '*')
                        a *= b;
                    else
                        a /= b;
                    step += a;
                    output.Add(step);
                }
                else
                {
                    output.Add("Ошибка - " + parts[i].Substring(1) + ".");
                    break;
                }
            }
            return output;
        }
    }
}
