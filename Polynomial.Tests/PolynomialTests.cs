namespace Polynomial.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "StyleCop.SA1600", Justification = "")]
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
