namespace Polynomial
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
        /// Thrown if argument has no values.
        /// </exception>
        public Polynomial(params double[] coefficients)
        {
            ThrowForInvalidParameter();

            this.coefficients = coefficients;

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
    }
}
