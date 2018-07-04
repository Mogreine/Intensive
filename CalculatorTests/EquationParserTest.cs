using System;
using System.Collections.Generic;
using CalculatorAlex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTests
{
    [TestClass]
    public class EquationParserTest
    {

        [TestMethod]
        public void StepsTest1()
        {
            var actual = EquationParser.Steps("3 + 5");
            var excepted = new List<string>() { "3 + 5 = 8" };
            Assert.AreEqual(excepted, actual);
        }
    }
}
