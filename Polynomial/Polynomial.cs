namespace Polynomial
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <inheritdoc />
    /// <summary>
    /// Class for working with mathematical polynomials.
    /// </summary>
    public class Polynomial : IEquatable<Polynomial>
    {
        #region Private fields

        /// <summary>
        /// The comparison epsilon.
        /// </summary>
        private static readonly double COMPARISON_EPSILON = 10e-10;

        /// <summary>
        /// Polynomial coefficients.
        /// </summary>
        private readonly double[] coefficients;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> class.
        /// Value at 0 index will be coefficient at the 
        /// highest degree, value at last index will be the free member. 
        /// </summary>
        /// <param name="coefficients">
        /// Polynomial coefficient.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if argument is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if argument has no values.
        /// </exception>
        public Polynomial(params double[] coefficients)
        {
            ThrowForInvalidParameter();

            int i;
            for (i = 0; i < coefficients.Length - 1; i++)
            {
                if (!IsEqualDoubles(coefficients[i], 0d))
                {
                    break;
                }
            }

            int coeffsLength = coefficients.Length - i;
            this.coefficients = new double[coeffsLength];

            for (int j = 0; j < coeffsLength; j++)
            {
                this.coefficients[j] = IsEqualDoubles(coefficients[i], 0d) ? 0 : coefficients[i];
                i++;
            }

            void ThrowForInvalidParameter()
            {
                if (coefficients == null)
                {
                    throw new ArgumentNullException(nameof(coefficients));
                }

                if (coefficients.Length == 0)
                {
                    throw new ArgumentException($"{nameof(coefficients)} must have at least one element", nameof(coefficients));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets polynomial coefficients.
        /// </summary>
        public double[] Coefficients
        {
            get
            {
                var copy = new double[this.coefficients.Length];
                for (int i = 0; i < copy.Length; i++)
                {
                    copy[i] = this.coefficients[i];
                }

                return copy;
            }
        }

        #endregion

        #region Operators overloads

        /// <summary>
        /// Comparison operator.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// True if both operands are null or if all they coefficients match.
        /// False otherwise.
        /// </returns>
        public static bool operator ==(Polynomial lhs, Polynomial rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }

            if (lhs.GetAmountOfCoefficients() != rhs.GetAmountOfCoefficients())
            {
                return false;
            }

            // Same coefficients.
            for (int i = 0; i < lhs.Coefficients.Length; i++)
            {
                if (!IsEqualDoubles(lhs.Coefficients[i], rhs.Coefficients[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Comparison operator.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// True if only one of operands is null or not all coefficients are same.
        /// False otherwise.
        /// </returns>
        public static bool operator !=(Polynomial lhs, Polynomial rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Addition operator.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// Polynomial which coefficients are sums of corresponding operand's coefficients.
        /// </returns>
        public static Polynomial operator +(Polynomial lhs, Polynomial rhs)
        {
            bool leftIsShortest = lhs.GetAmountOfCoefficients() < rhs.GetAmountOfCoefficients();

            double[] shortestCoeffs = leftIsShortest ? lhs.Coefficients : rhs.Coefficients;
            double[] longestCoeffs = leftIsShortest ? rhs.Coefficients : lhs.Coefficients;

            int diff = longestCoeffs.Length - shortestCoeffs.Length;
            var newPolynomCoeffs = new double[longestCoeffs.Length];
            Array.Copy(longestCoeffs, newPolynomCoeffs, diff);

            for (int i = diff; i < newPolynomCoeffs.Length; i++)
            {
                newPolynomCoeffs[i] = shortestCoeffs[i - diff] + longestCoeffs[i];
            }

            return new Polynomial(newPolynomCoeffs);
        }

        /// <summary>
        /// Subtraction operator.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// Polynomial which coefficients are differences of corresponding operand's coefficients.
        /// </returns>
        public static Polynomial operator -(Polynomial lhs, Polynomial rhs)
        {
            bool leftIsShortest = lhs.GetAmountOfCoefficients() < rhs.GetAmountOfCoefficients();

            double[] shortestCoeffs = leftIsShortest ? lhs.Coefficients : rhs.Coefficients;
            double[] longestCoeffs = leftIsShortest ? rhs.Coefficients : lhs.Coefficients;
            var newPolynomCoeffs = new double[longestCoeffs.Length];
            int diff = longestCoeffs.Length - shortestCoeffs.Length;

            if (leftIsShortest)
            {
                SubtractShortestFromLongest();
            }
            else
            {
                SubtractLongestFromShortest();
            }

            return new Polynomial(newPolynomCoeffs);

            void SubtractLongestFromShortest()
            {
                Array.Copy(longestCoeffs, newPolynomCoeffs, longestCoeffs.Length);
                for (int i = diff; i < newPolynomCoeffs.Length; i++)
                {
                    newPolynomCoeffs[i] -= shortestCoeffs[i - diff];
                }
            }

            void SubtractShortestFromLongest()
            {
                Array.Copy(shortestCoeffs, 0, newPolynomCoeffs, diff, shortestCoeffs.Length);
                for (int i = 0; i < newPolynomCoeffs.Length; i++)
                {
                    newPolynomCoeffs[i] -= longestCoeffs[i];
                }
            }
        }

        /// <summary>
        /// Negation operator.
        /// </summary>
        /// <param name="polynomial">
        /// The polynomial that will be negated.
        /// </param>
        /// <returns>
        /// New Polynomial object with negated coefficients.
        /// </returns>
        public static Polynomial operator -(Polynomial polynomial)
        {
            double[] newCoeffs = polynomial.Coefficients;
            for (int i = 0; i < newCoeffs.Length; i++)
            {
                newCoeffs[i] *= -1;
            }

            return new Polynomial(newCoeffs);
        }

        /// <summary>
        /// Multiplication of two polynomials.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// Polynomial that is the result of multiplying given polynomials.
        /// </returns>
        public static Polynomial operator *(Polynomial lhs, Polynomial rhs)
        {
            int resultLength = lhs.GetAmountOfCoefficients() + rhs.GetAmountOfCoefficients() - 1;
            var resultCoeffs = new double[resultLength];
            int shift = 0;
            int leftLength = lhs.GetAmountOfCoefficients();
            for (int i = rhs.GetAmountOfCoefficients() - 1; i >= 0; i--)
            {
                int lastIndex = resultLength - shift;
                int firstIndex = lastIndex - leftLength;
                for (int j = firstIndex; j < lastIndex; j++)
                {
                    resultCoeffs[j] += lhs.coefficients[j - firstIndex] * rhs.coefficients[i];
                }

                shift++;
            }

            return new Polynomial(resultCoeffs);
        }

        /// <summary>
        /// Multiplication by number.
        /// </summary>
        /// <param name="lhs">
        /// Polynomial which coefficients will be multiplied.
        /// </param>
        /// <param name="rhs">
        /// Coefficient to which given polynomial will be multiplied.
        /// </param>
        /// <returns>
        /// Multiplied polynomial.
        /// </returns>
        public static Polynomial operator *(Polynomial lhs, double rhs)
        {
            double[] coeffs = lhs.Coefficients;
            for (int i = 0; i < coeffs.Length; i++)
            {
                coeffs[i] *= rhs;
            }

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Multiplication by number.
        /// </summary>
        /// <param name="lhs">
        /// Coefficient to which given polynomial will be multiplied.
        /// </param>
        /// <param name="rhs">
        /// Polynomial which coefficients will be multiplied.
        /// </param>
        /// <returns>
        /// Multiplied polynomial.
        /// </returns>
        public static Polynomial operator *(double lhs, Polynomial rhs)
        {
            return rhs * lhs;
        }

        #endregion

        #region CLR-Complant operators methods

        /// <summary>
        /// Add two polynomials.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Polynomial which coefficients are sums of corresponding operand's coefficients.
        /// </returns>
        public static Polynomial Add(Polynomial lhs, Polynomial rhs) => lhs + rhs;

        /// <summary>
        /// Substract two polynomials.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Polynomial which coefficients are differences of corresponding operand's coefficients.
        /// </returns>
        public static Polynomial Substract(Polynomial lhs, Polynomial rhs) => lhs - rhs;

        /// <summary>
        /// Multiply two polynomials.
        /// </summary>
        /// <param name="lhs">
        /// Left operand.
        /// </param>
        /// <param name="rhs">
        /// Right operand.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Polynomial that is the result of multiplying given polynomials.
        /// </returns>
        public static Polynomial Multiply(Polynomial lhs, Polynomial rhs) => lhs * rhs;

        /// <summary>
        /// Multiply polynomial by some coefficient.
        /// </summary>
        /// <param name="lhs">
        /// Coefficient to which given polynomial will be multiplied.
        /// </param>
        /// <param name="rhs">
        /// Polynomial which coefficients will be multiplied.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Multiplied polynomial.
        /// </returns>
        public static Polynomial Multiply(double lhs, Polynomial rhs) => lhs * rhs;

        /// <summary>
        /// Multiply polynomial by some coefficient.
        /// </summary>
        /// <param name="lhs">
        /// Polynomial which coefficients will be multiplied.
        /// </param>
        /// <param name="rhs">
        /// Coefficient to which given polynomial will be multiplied.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Multiplied polynomial.
        /// </returns>
        public static Polynomial Multiply(Polynomial lhs, double rhs) => lhs * rhs;

        /// <summary>
        /// Negate polynomial.
        /// </summary>
        /// <param name="polynomial">
        /// The polynomial.
        /// </param>
        /// <returns>
        /// The <see cref="Polynomial"/>.
        /// Polynomial object with negated coefficients.
        /// </returns>
        public static Polynomial Negate(Polynomial polynomial) => -polynomial;

        #endregion

        #region Custom public methods

        /// <summary>
        /// Returns amount of coefficient that this polynomial have.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetAmountOfCoefficients() => this.coefficients.Length;

        #endregion

        #region Object's methods overloads

        /// <summary>
        /// Converts polynomial to it's string representation.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// Polynomial's string representation.
        /// </returns>
        public override string ToString()
        {
            if (this.coefficients.Length == 1)
            {
                return this.coefficients[0].ToString(CultureInfo.CurrentCulture);
            }

            var result = new StringBuilder();

            this.FillFirstTerm(result);
            this.FillWithMiddleTerms(result);
            this.FillLastTerm(result);

            return result.ToString();
        }

        /// <summary>
        /// Returns a boolean indicating if 
        /// the passed in object is Equal to this. 
        /// </summary>
        /// <param name="obj">
        /// Object that will be compared with this.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj is Polynomial p && this == p;
        }

        /// <summary>
        /// Serves as default hash function.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// Current object's hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return this.coefficients != null ? this.coefficients.GetHashCode() : 0;
        }

        #endregion

        #region IEquatable's implementation

        /// <summary>
        /// IEquatable's Equals method.
        /// </summary>
        /// <param name="other">
        /// Polynomial with which you want to compare the current.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if polynomials are both null or their coefficients are equal.
        /// False otherwise.
        /// </returns>
        public bool Equals(Polynomial other)
        {
            return this == other;
        }

        #endregion

        #region Static private helpers

        /// <summary>
        /// Checks if two double values are equal with some accuracy.
        /// </summary>
        /// <param name="a">
        /// First double value.
        /// </param>
        /// <param name="b">
        /// Second double value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if a and b are equal, false otherwise.
        /// </returns>
        private static bool IsEqualDoubles(double a, double b) => Math.Abs(a - b) < COMPARISON_EPSILON;

        #endregion

        #region ToString private helpers

        /// <summary>
        /// Appends first term to passed StringBuilder.
        /// </summary>
        /// <param name="result">
        /// StringBuilder to which first term would be appended.
        /// </param>
        private void FillFirstTerm(StringBuilder result)
        {
            int maxPower = this.coefficients.Length - 1;
            var firstTerm = maxPower > 1 ? $"x^{maxPower}" : "x";

            if (!IsEqualDoubles(this.coefficients[0], 1))
            {
                firstTerm = this.coefficients[0] + firstTerm;
            }

            result.Append(firstTerm);
        }

        /// <summary>
        /// Appends middle terms to passed StringBuilder.
        /// </summary>
        /// <param name="result">
        /// StringBuilder to which middle terms would be appended.
        /// </param>
        private void FillWithMiddleTerms(StringBuilder result)
        {
            for (int index = 1; index < this.coefficients.Length - 1; index++)
            {
                if (IsEqualDoubles(this.coefficients[index], 0))
                {
                    continue;
                }

                string currentAbsoluteTerm = this.GetAbsoluteTerm(index);

                string sign = this.coefficients[index] < 0 ? " - " : " + ";
                result.Append(sign);
                result.Append(currentAbsoluteTerm);
            }
        }

        /// <summary>
        /// Gets absolute value of a given term.
        /// </summary>
        /// <param name="index">
        /// Term index.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// Absolute value of a given term as string.
        /// </returns>
        private string GetAbsoluteTerm(int index)
        {
            int power = this.coefficients.Length - 1 - index;

            var result = power != 1 ? $"x^{power}" : "x";

            if (!IsEqualDoubles(this.coefficients[index], 1))
            {
                result = Math.Abs(this.coefficients[index]) + result;
            }

            return result;
        }

        /// <summary>
        /// Appends last term to passed StringBuilder.
        /// </summary>
        /// <param name="result">
        /// StringBuilder to which last term would be appended.
        /// </param>
        private void FillLastTerm(StringBuilder result)
        {
            int lastIndex = this.coefficients.Length - 1;
            double coefficient = this.coefficients[lastIndex];
            if (IsEqualDoubles(coefficient, 0))
            {
                return;
            }

            string sign = coefficient < 0 ? " - " : " + ";
            result.Append(sign + Math.Abs(coefficient));
        }

        #endregion
    }
}
