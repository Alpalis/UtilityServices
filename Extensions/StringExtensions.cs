using System;

namespace Alpalis
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToOnlyFirstCharUpper(this string input)
        {
            return input.ToLower().FirstCharToUpper();
        }
    }
}
