namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Language validation exception
    /// </summary>
    /// <seealso cref="MabService.Shared.Exceptions.ValidationException" />
    public class LanguageValidationException : ValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageValidationException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public LanguageValidationException(string error) : base(error)
        {
        }
    }
}
