namespace Polynomial
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class for working with mathematical polynomials.
    /// </summary>
    public class Polynomial : IEquatable<Polynomial>
    {
        #region Private fields

        /// <summary>
        /// Gets polynomial coefficients.
        /// </summary>
        public double[] Coefficients { get; private set; }

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
        /// Thrown if argument has no values or all value are equal to zero.
        /// </exception>
        public Polynomial(params double[] coefficients)
        {
            ThrowForInvalidParameter();

            int i;
            for (i = 0; i < coefficients.Length; i++)
            {
                if (coefficients[i] != 0)
                {
                    break;
                }
            }

            int coeffsLength = coefficients.Length - i;
            this.Coefficients = new double[coeffsLength];
            Array.Copy(coefficients, i, this.Coefficients, 0, coeffsLength);

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

                if (Array.TrueForAll(coefficients, v => v == 0))
                {
                    throw new ArgumentException("All coefficients cannot be equal to zero!", nameof(coefficients));
                }
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

            if (lhs.Coefficients.Length != rhs.Coefficients.Length)
            {
                return false;
            }

            for (int i = 0; i < lhs.Coefficients.Length; i++)
            {
                if (lhs.Coefficients[i] != rhs.Coefficients[i])
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
            if (this.Coefficients.Length == 1)
            {
                return this.Coefficients[0].ToString();
            }

            var result = new StringBuilder();

            this.FillFirstTerm(result);
            this.FillWithMiddleTerms(result);
            this.FillLastTerm(result);

            return result.ToString();
        }

        /// <summary>
        /// Returns a boolean indicating if 
        /// the passed in object obj is Equal to this. 
        /// </summary>
        /// <param name="obj">
        /// The obj.
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
            return this.Coefficients != null ? this.Coefficients.GetHashCode() : 0;
        }

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
            if (this.Coefficients[0] == 0)
            {
                return;
            }

            int maxPower = this.Coefficients.Length - 1;
            var firstTerm = $"x^{maxPower}";

            if (this.Coefficients[0] != 1)
            {
                firstTerm = this.Coefficients[0] + firstTerm;
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
            for (int index = 1; index < this.Coefficients.Length - 1; index++)
            {
                if (this.Coefficients[index] == 0)
                {
                    continue;
                }

                string currentAbsoluteTerm = this.GetAbsoluteTerm(index);

                string sign = this.Coefficients[index] < 0 ? " - " : " + ";
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
            if (this.Coefficients[index] == 0)
            {
                return string.Empty;
            }

            int power = this.Coefficients.Length - 1 - index;

            var result = power != 1 ? $"x^{power}" : "x";

            if (this.Coefficients[index] != 1)
            {
                result = Math.Abs(this.Coefficients[index]) + result;
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
            int lastIndex = this.Coefficients.Length - 1;
            double coefficient = this.Coefficients[lastIndex];
            if (coefficient == 0)
            {
                return;
            }

            string sign = coefficient < 0 ? " - " : " + ";
            result.Append(sign + coefficient);
        }

        #endregion

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
    }
}
