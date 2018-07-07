namespace Polynomial.Tests
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class PolynomialTests
    {
        [TestCase(1.5, 2, 2, ExpectedResult = "1,5x^2 + 2x + 2")]
        [TestCase(1, 2, 0, ExpectedResult = "x^2 + 2x")]
        [TestCase(100, 0, 1, ExpectedResult = "100x^2 + 1")]
        [TestCase(-200, -1, 0.01, ExpectedResult = "-200x^2 - 1x + 0,01")]
        [TestCase(0,0,0,1, ExpectedResult = "1")]
        public string ToString_ValidInput_ValidResult(params double[] coeffs)
        {
            var p = new Polynomial(coeffs);
            return p.ToString();
        }

        [TestCase(new[] { 1d, 2, 3 }, new[] { 1d, 2, 3 }, new[] { 2d, 4, 6 })]
        [TestCase(new[] { 0d, 2, 3 }, new[] { 1d, 2 }, new[] { 3d, 5 })]
        [TestCase(new[] { 0d, 2, 0 }, new[] { -1d, 0 }, new[] { 1d, 0 })]
        [TestCase(new[] { 0d, 3 }, new[] { 1.5, -12 }, new[] { 1.5, -9 })]
        [TestCase(new[] { -5.25, 12, -43 }, new[] { 5.25, 0, 1 }, new[] { 0d, 12, -42 })]
        public void Addition_ValieInput_ValidOutput(double[] d1, double[] d2, double[] sum)
        {
            var p1 = new Polynomial(d1);
            var p2 = new Polynomial(d2);

            var expected = new Polynomial(sum);

            Polynomial actual = p1 + p2;

            Assert.True(actual == expected);
        }

        [Test]
        public void Constructor_PassedNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Polynomial(null));

        [Test]
        public void Constructor_PassedNoValues_ThrowsArgumentException() =>
            Assert.Throws<ArgumentException>(() => new Polynomial());

        [Test]
        public void Constructor_PassedAllZeros_ThrowsArgumentException() =>
            Assert.Throws<ArgumentException>(() => new Polynomial(0, 0, 0, 0));
    }
}
