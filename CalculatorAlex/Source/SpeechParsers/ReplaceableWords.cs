﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace CalculatorAlex
{
    class ReplaceableWords
    {
        public static Dictionary<string, string> OperationsRu = new Dictionary<string, string>
        {
                {"x", "*" },
                {"х", "*" },
                {"плюс-минус ", "+ -" },
                {"умножить на", "*"},
                {"разделить на", "/"},
                {"поделить на", "/"},
                {"делить на", "/"},
                {"запятая", "."},
                {"точка", "."},
                {"умножить", "*"},
                {"делить", "/"},
                {"плюс", "+"},
                {"минус", "-"},
                {"minus", "-"},
                {"plus", "+"},
                {"ноль", "0"},
                {"нуль", "0"},
                {",", "."}
        };

        public static Dictionary<string, double> BigNumbRu = new Dictionary<string, double>
        {
                {"один", 1},
                {"млн", 1000000},
                {"миллионов", 1000000},
                {"миллион", 1000000},
                {"миллиона", 1000000},
                {"миллиард", 1000000000},
                {"миллиарда", 1000000000},
                {"миллиардов", 1000000000},
                {"млрд", 1000000000}
        };

        public static Dictionary<string, string> OperationsEn = new Dictionary<string, string>
        {
                {"x", "*" },
                {"х", "*" },
                {"free", "3" },
                {"minus", "-"},
                {"plus", "+"},
                {"divided", "/"},
                {"divide", "/"},
                {"multiply", "*"},
                {",", "."}
        };

        public static Dictionary<string, double> BigNumbEn = new Dictionary<string, double>
        {
                {"million", 1000000},
                {"billion", 1000000000}
        };

        public static SpeechContext RussianContext = new SpeechContext
        {
            Phrases = { "миллион", "миллиард", "разделить на", "делить на", "разделить", "умножить на", "умножить" }
        };

        public static SpeechContext EnglishContext = new SpeechContext
        {
            Phrases = { "twelve", "divide", "three" }
        };
    }
}
