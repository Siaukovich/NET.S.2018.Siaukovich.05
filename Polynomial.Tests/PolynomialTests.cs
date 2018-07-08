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
        [TestCase(0, 0, 0, 1, ExpectedResult = "1")]
        [TestCase(0, 5, -1, ExpectedResult = "5x - 1")]
        [TestCase(0, 2, 0, ExpectedResult = "2x")]
        [TestCase(0, 0, 0, ExpectedResult = "0")]
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
        public void Addition_ValidInput_ValidOutput(double[] d1, double[] d2, double[] sum)
        {
            var p1 = new Polynomial(d1);
            var p2 = new Polynomial(d2);

            var expected = new Polynomial(sum);

            Polynomial actual = p1 + p2;

            Assert.True(actual == expected, $"{actual} != {expected}");
        }

        [TestCase(new[] { 1d, 2, 3 }, new[] { 1d, 2, 3 }, new[] { 0d })]
        [TestCase(new[] { 0d, 2, 3 }, new[] { 1d, 2 }, new[] { 1d, 1 })]
        [TestCase(new[] { 0d, 2, 0 }, new[] { -1d, 0 }, new[] { 3d, 0 })]
        [TestCase(new[] { 0d, 3 }, new[] { 1.5, -12 }, new[] { -1.5, 15 })]
        [TestCase(new[] { -5.25, 12, -43 }, new[] { 5.25, 0, 1 }, new[] { -10.5, 12, -44 })]
        [TestCase(new[] { 0d, 1, 9, 3, -43 }, new[] { 8.8, 0, -5 }, new[] { 1d, 0.2, 3, -38 })]
        public void Subtraction_ValidInput_ValidOutput(double[] d1, double[] d2, double[] sum)
        {
            var p1 = new Polynomial(d1);
            var p2 = new Polynomial(d2);

            var expected = new Polynomial(sum);

            Polynomial actual = p1 - p2;

            Assert.True(actual == expected, $"{actual} != {expected}");
        }

        [TestCase(new[] { 1d, 2, 3 }, 3d, new[] { 3d, 6, 9 })]
        [TestCase(new[] { 0d, 2, 3 }, 0.5, new[] { 1, 1.5 })]
        [TestCase(new[] { 0d, 2, 0 }, 0, new[] { 0d })]
        [TestCase(new[] { 0d, 3 }, -1, new[] { -3d })]
        [TestCase(new[] { -5d, 12, -43 }, 11d , new[] { -55d, 132, -473 })]
        public void MultiplicationWithCoefficient_ValidInput_ValidOutput(double[] d, double coeff, double[] result)
        {
            var p = new Polynomial(d);

            var expected = new Polynomial(result);

            Polynomial actual_left = p * coeff;
            Polynomial actual_right = coeff * p;

            Assert.True(actual_left == expected, $"{actual_left} != {expected}");
            Assert.True(actual_right == expected, $"{actual_right} != {expected}");
        }

        [TestCase(new[] { 5d, 8, 2 }, new[] { 2d, -1 }, new[] { 10d, 11, -4, -2 })]
        [TestCase(new[] { 5d, 0, 0, 2 }, new[] { 1d, -5, 4, 0 }, new[] { 5d, -25, 20, 2, -10, 8, 0 })]
        [TestCase(new[] { 3d, 0, -2 }, new[] { 1d, -2, -8 }, new[] { 3d, -6, -26, 4, 16 })]
        [TestCase(new[] { 3d, 0, -2 }, new[] { 0d }, new[] { 0d })]
        [TestCase(new[] { 3d, 0, -2 }, new[] { 1d }, new[] { 3d, 0, -2 })]
        public void Multiplication_ValidInput_ValidOutput(double[] d1, double[] d2, double[] mul)
        {
            var p1 = new Polynomial(d1);
            var p2 = new Polynomial(d2);

            var expected = new Polynomial(mul);

            Polynomial actual = p1 * p2;

            Assert.True(actual == expected, $"{actual} != {expected}");
        }

        [TestCase(new[] { 3d, 0, -2 })]
        [TestCase(new[] { -3d, 0, 2 })]
        [TestCase(new[] { 0d })]
        public void Negation_ValidInput_ValidOutput(double[] coeffs)
        {
            double[] negatedCoeffs = coeffs.Select(v => -v).ToArray();
            Polynomial expected = new Polynomial(negatedCoeffs);

            Polynomial actual = -new Polynomial(coeffs);

            Assert.IsTrue(expected == actual, $"{expected} != {actual}");
        }

        [Test]
        public void Constructor_PassedNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Polynomial(null));

        [Test]
        public void Constructor_PassedNoValues_ThrowsArgumentException() =>
            Assert.Throws<ArgumentException>(() => new Polynomial());
    }
}
