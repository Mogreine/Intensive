using CalculatorAlex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTests
{
    [TestClass]
    public class DoubleParserTest
    {
        double a;

        [TestMethod]
        public void TryParseTest1()
        {
            var excepted = DoubleParser.TryParse("15", out a);
            var actual = true;
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TryParseTest2()
        {
            var excepted = DoubleParser.TryParse("15.45", out a);
            var actual = true;
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TryParseTest3()
        {
            var excepted = DoubleParser.TryParse("+", out a);
            var actual = false;
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TryParseTest4()
        {
            var excepted = DoubleParser.TryParse("привет", out a);
            var actual = false;
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TryParseTest5()
        {
            var excepted = DoubleParser.TryParse("hello", out a);
            var actual = false;
            Assert.AreEqual(excepted, actual);
        }
    }
}
