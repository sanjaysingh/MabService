using System;
using System.Linq;
namespace MabService.Shared
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether [is null or white space].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Determines whether this instance is null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this string str)
        {
            return str == null;
        }

        /// <summary>
        /// Determines whether this instance is alphanumeric.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is not alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotAlphanumeric (this string str)
        {
            return str.Any(c => !char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Determines whether [is not in length] [the specified minimum].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>
        ///   <c>true</c> if [is not in length] [the specified minimum]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotInLength(this string str,  int min, int max)
        {
            return str.Length < min || str.Length > max;
        }

        /// <summary>
        /// To the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
