namespace Polynomial
{
    using System;
    using System.Text;

    /// <summary>
    /// Class for working with mathematical polynomials.
    /// </summary>
    public class Polynomial
    {
        #region Private fields

        /// <summary>
        /// Polynomial's coefficients.
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
            this.coefficients = new double[coeffsLength];
            Array.Copy(coefficients, i, this.coefficients, 0, coeffsLength);

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
                return this.coefficients[0].ToString();
            }

            var result = new StringBuilder();

            this.FillFirstTerm(result);
            this.FillWithMiddleTerms(result);
            this.FillLastTerm(result);

            return result.ToString();
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
            if (this.coefficients[0] == 0)
            {
                return;
            }

            int maxPower = this.coefficients.Length - 1;
            var firstTerm = $"x^{maxPower}";

            if (this.coefficients[0] != 1)
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
                if (this.coefficients[index] == 0)
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
            if (this.coefficients[index] == 0)
            {
                return string.Empty;
            }

            int power = this.coefficients.Length - 1 - index;

            var result = power != 1 ? $"x^{power}" : "x";

            if (this.coefficients[index] != 1)
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
            if (coefficient == 0)
            {
                return;
            }

            string sign = coefficient < 0 ? " - " : " + ";
            result.Append(sign + coefficient);
        }

        #endregion
    }
}
