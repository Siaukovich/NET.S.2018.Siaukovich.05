﻿namespace Polynomial
{
    using System;
    using System.Text;

    /// <summary>
    /// Class for working with mathematical polynomials.
    /// </summary>
    public class Polynomial
    {
        /// <summary>
        /// Polynomial's coefficients.
        /// </summary>
        private readonly double[] coefficients;

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

        /// <summary>
        /// Converts polynomial to it's string representation.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// Polynomial's string representation.
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            string firstTerm = this.GetFirstTerm();
            result.Append(firstTerm);

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

            string lastTerm = GetLastTerm();
            result.Append(lastTerm);

            return result.ToString();
        }

        private string GetFirstTerm()
        {
            if (this.coefficients[0] == 0)
            {
                return string.Empty;
            }

            int maxPower = this.coefficients.Length - 1;
            var result = $"x^{maxPower}";

            if (this.coefficients[0] != 1)
            {
                result = this.coefficients[0] + result;
            }

            return result;
        }

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

        private string GetLastTerm()
        {
            int lastIndex = this.coefficients.Length - 1;
            double coefficient = this.coefficients[lastIndex];
            if (coefficient == 0)
            {
                return string.Empty;
            }

            string sign = coefficient < 0 ? " - " : " + ";
            return sign + coefficient.ToString();
        }
    }
}
