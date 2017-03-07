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
        ///   <c>true</c> if the specified string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphanumeric (this string str)
        {
            return !str.IsNullOrWhiteSpace() && str.All(c => char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Determines whether [is in length] [the specified minimum].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>
        ///   <c>true</c> if [is in length] [the specified minimum]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInLength(this string str,  int min, int max)
        {
            return !str.IsNull() && str.Length >= min && str.Length <= max;
        }
    }
}
