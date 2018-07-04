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
            CollectionAssert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void StepsTest2()
        {
            var actual = EquationParser.Steps("3 + 5 / 4 * 2");
            var excepted = new List<string>() { "3 + 5 = 8", "8 / 4 = 2", "2 * 2 = 4" };
            CollectionAssert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void StepsTest3()
        {
            var actual = EquationParser.Steps("3.5 + 1.5 / 2.5 * 3.75");
            var excepted = new List<string>() { "3.5 + 1.5 = 5", "5 / 2.5 = 2", "2 * 3.75 = 7.5" };
            CollectionAssert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void StepsTest4()
        {
            var actual = EquationParser.Steps("привет");
            var excepted = new List<string>() { "Выражение привет составлено неправильно." };
            CollectionAssert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void StepsTest5()
        {
            var actual = EquationParser.Steps("3 + 7 * 2.5 + привет");
            var excepted = new List<string>() { "Выражение 3 + 7 * 2.5 + привет составлено неправильно." };
            CollectionAssert.AreEqual(excepted, actual);
        }
    }
}
