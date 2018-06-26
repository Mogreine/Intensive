using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    class ExpParser
    {
        private static double GetNumAns(string str)
        {
            double ans = 0;
            string []piece = str.Split(' ');
            foreach (var p in piece)
            {
                switch (p[0])
                {
                    case '*':
                        ans *= int.Parse(p.Substring(1));
                        break;
                    case '/':
                        ans /= int.Parse(p.Substring(1));
                        break;
                    case '-':
                        ans -= int.Parse(p.Substring(1));
                        break;
                    case '+':
                        ans += int.Parse(p.Substring(1));
                        break;
                    default:
                        ans += int.Parse(p.Substring(1));
                        break;
                }
            }
            return ans;
        }
    }
}
