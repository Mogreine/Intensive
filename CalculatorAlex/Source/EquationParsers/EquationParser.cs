﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAlex
{
    public class EquationParser
    {

        public static bool Success;
        public static List<string> AllValues;

        public static List<string> Steps(string expression)
        {
            Success = true;
            AllValues = new List<string>();
            var output = new List<string>();
            var parts = expression.Split(' ');
            double res;
            if (DoubleParser.TryParse(parts[0], out res))
            {
                for (var i = 1; i < parts.Length - 1; i += 2)
                {
                    var step = res.ToString(Culture.EngInfo) + " " + parts[i] + " " + parts[i + 1] + " = ";
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
                            if (nextOperand == 0)
                            {
                                AllValues.Clear();
                                output.Clear();
                                Success = false;
                                output.Add("Выражение " + expression + " составлено неправильно.");
                                break;
                            }
                            res /= nextOperand;
                            res = Math.Round(res, 3);
                        }
                        step += res.ToString(Culture.EngInfo);
                        AllValues.Add(res.ToString(Culture.EngInfo));
                        output.Add(step);
                    }
                    else
                    {
                        AllValues.Clear();
                        output.Clear();
                        Success = false;
                        output.Add("Выражение " + expression + " составлено неправильно.");
                        break;
                    }
                }
            }
            else
            {
                AllValues.Clear();
                output.Clear();
                Success = false;
                output.Add("Выражение " + expression + " составлено неправильно.");
            }

            if (output.Count == 0)
            {
                AllValues.Add(res.ToString(Culture.EngInfo));
                output.Add(res.ToString(Culture.EngInfo));
            }
            return output;
        }
    }
}
