using CalculatorAlex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTests
{
    [TestClass]
    public class ConverterTest
    {
        Converter c1 = new Converter("ru-RU");
        Converter c2 = new Converter("en-US");

        [TestMethod]
        public void TransformBigNumbers1()
        {
            var actual = c1.TransformBigNumbers("11134535");
            var excepted = "11134535";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers2()
        {
            var actual = c1.TransformBigNumbers("   11134535  ");
            var excepted = "11134535";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers3()
        {
            var actual = c1.TransformBigNumbers("12334535.34");
            var excepted = "12334535.34";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers4()
        {
            var actual = c1.TransformBigNumbers("   12334535.34  ");
            var excepted = "12334535.34";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers5()
        {
            var actual = c1.TransformBigNumbers("один миллион");
            var excepted = "1000000";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers6()
        {
            var actual = c1.TransformBigNumbers("один миллион 13");
            var excepted = "1000013";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers7()
        {
            var actual = c1.TransformBigNumbers("один миллион 13.67");
            var excepted = "1000013,67";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers8()
        {
            var actual = c2.TransformBigNumbers("million");
            var excepted = "1000000";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers9()
        {
            var actual = c2.TransformBigNumbers("million 13");
            var excepted = "1000013";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void TransformBigNumbers10()
        {
            var actual = c2.TransformBigNumbers("million 13.67");
            var excepted = "1000013,67";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation1()
        {
            var actual = c1.ConvertTextToEquation("31 + 5");
            var excepted = "31 + 5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation2()
        {
            var actual = c1.ConvertTextToEquation("31 +   5 ");
            var excepted = "31 + 5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation3()
        {
            var actual = c1.ConvertTextToEquation("31 плюс 5");
            var excepted = "31 + 5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation4()
        {
            var actual = c1.ConvertTextToEquation("31 плюс-минус 5");
            var excepted = "31 + -5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation5()
        {
            var actual = c1.ConvertTextToEquation("5,67");
            var excepted = "5.67";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation6()
        {
            var actual = c2.ConvertTextToEquation("31 plus 5");
            var excepted = "31 + 5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation7()
        {
            var actual = c2.ConvertTextToEquation("31 plus    5");
            var excepted = "31 + 5";
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void ConvertTextToEquation8()
        {
            var actual = c2.ConvertTextToEquation("free");
            var excepted = "3";
            Assert.AreEqual(excepted, actual);
        }

    }
}
