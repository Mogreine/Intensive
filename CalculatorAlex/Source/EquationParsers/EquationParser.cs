using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class EquationParser
    {

        public static double? Parse(string str)
        {
            var piece = str.Split(' ');

            double ans;
            if (!Double.TryParse(piece[0], out ans))
            {
                return null;
            }

            for (var i = 1; i < piece.Length; i++)
            {
                double operand;
                if (!Double.TryParse(piece[i].Substring(1), out operand))
                {
                    return null;
                }
                switch (piece[i][0])
                {
                    case '*':
                        ans *= operand;
                        break;
                    case '/':
                        ans /= operand;
                        break;
                    case '-':
                        ans -= operand;
                        break;
                    case '+':
                        ans += operand;
                        break;
                    default:
                        return null;
                }
            }
            return ans;
        }
    }
}
