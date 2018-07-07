namespace Extension
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class that extends string class functionality.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts given number as string to decimal base.
        /// </summary>
        /// <param name="str">
        /// Positive number as string.
        /// </param>
        /// <param name="strBase">
        /// Base of given number.
        /// </param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// Given number in base 10.
        /// </returns>
        /// <exception cref="OverflowException">
        /// Thrown if given number doesn't fit in UInt32.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if passed string is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if strBase not in range [2, 16].
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if string number contains symbols that are not used in given base.
        /// </exception>
        public static uint ToDecimalBase(this string str, int strBase)
        {
            var elements = new Dictionary<string, int>
            {
                { "0", 0 },
                { "1", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "A", 10 },
                { "B", 11 },
                { "C", 12 },
                { "D", 13 },
                { "E", 14 },
                { "F", 15 }
            };

            ThrowForInvalidParameters();

            try
            {
                return ConvertToDecimal(str, strBase, elements);
            }
            catch (OverflowException)
            {
                throw new OverflowException("Value can not fit in UInt32 value");
            }

            void ThrowForInvalidParameters()
            {
                if (str == null)
                {
                    throw new ArgumentNullException(nameof(str));
                }

                const int MIN_BASE = 2;
                const int MAX_BASE = 16;
                if (strBase < MIN_BASE || strBase > MAX_BASE)
                {
                    throw new ArgumentOutOfRangeException(nameof(strBase), $"{strBase} should be in range [{MIN_BASE}, {MAX_BASE}].");
                }

                foreach (char c in str)
                {
                    var element = c.ToString().ToUpper();
                    if (elements.ContainsKey(element) && elements[element] >= strBase)
                    {
                        throw new ArgumentException($"Element '{c}' is not in base {strBase} system.");
                    }
                }                
            }
        }

        private static uint ConvertToDecimal(string str, int strBase, Dictionary<string, int> dict)
        {
            uint answer = 0;
            int power = str.Length - 1;
            for (int i = 0; i < str.Length; i++)
            {
                var element = str[i].ToString().ToUpper();
                checked
                {
                    answer += (uint)(dict[element] * Math.Pow(strBase, power));
                }

                power--;
            }

            return answer;
        }
    }
}
