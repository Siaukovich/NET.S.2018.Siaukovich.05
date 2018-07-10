namespace Extension
{
    using System;
    using System.Collections.Generic;
    using System.Security.AccessControl;

    /// <summary>
    /// Class that extends string class functionality.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts given positive number from string representation to decimal base.
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
        public static int ToDecimalBase(this string str, int strBase)
        {
            ThrowForInvalidParameters();

            string elements = GetGivenBaseElements(strBase);
            ThrowForInvalidElementsInStr();

            return ConvertToDecimal(str, strBase, elements);

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
            }

            void ThrowForInvalidElementsInStr()
            {
                foreach (char c in str)
                {
                    var element = c.ToString().ToUpper();
                    if (!elements.Contains(element))
                    {
                        throw new ArgumentException($"Element '{c}' is not in base {strBase} system.");
                    }
                }
            }
        }

        /// <summary>
        /// Returns string that contains all elements used in passed numeric system.
        /// </summary>
        /// <param name="strBase">
        /// Numeric system base.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// Elements of given base.
        /// </returns>
        private static string GetGivenBaseElements(int strBase)
        {
            var elements = new char[strBase];

            int i;
            for (i = 0; i < 10 && i < strBase; i++)
            {
                elements[i] = i.ToChar();
            }

            const int MAX_BASE = 16;
            while (i < MAX_BASE && i < strBase)
            {
                int letterNumber = i - 10;
                elements[i] = letterNumber.ToCharLetter();
                i++;
            }

            return new string(elements);
        }

        /// <summary>
        /// Converts number passed as string to decimal base.
        /// </summary>
        /// <param name="str">
        /// Number as string.
        /// </param>
        /// <param name="strBase">
        /// Base of number.
        /// </param>
        /// <param name="dict">
        /// Dict, that contains mapping for base.
        /// </param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// Converted number.
        /// </returns>
        private static int ConvertToDecimal(string str, int strBase, string elements)
        {
            int answer = 0;
            long power = 1;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                var element = str[i].ToString().ToUpper();
                checked
                {
                    answer += (int) (elements.IndexOf(element) * power);
                    power *= strBase;
                }
            }

            return answer;
        }

        /// <summary>
        /// Converts digit to char.
        /// </summary>
        /// <param name="i">
        /// Digit.
        /// </param>
        /// <returns>
        /// The <see cref="char"/>.
        /// Passed digit as char.
        /// </returns>
        private static char ToChar(this int i)
        {
            const int OFFSET = 48;
            return (char)(i + OFFSET);
        }

        /// <summary>
        /// Converts integer to corresponding char letter.
        /// </summary>
        /// <param name="i">
        /// Number of letter in the alphabet.
        /// </param>
        /// <returns>
        /// The <see cref="char"/>.
        /// Letter.
        /// </returns>
        private static char ToCharLetter(this int i)
        {
            const int OFFSET = 65;
            return (char)(i + OFFSET);
        }
    }
}
