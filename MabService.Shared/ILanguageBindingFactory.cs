namespace MabService.Shared
{
    /// <summary>
    /// Language binding factory
    /// </summary>
    public interface ILanguageBindingFactory
    {
        /// <summary>
        /// Creates the language binding.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        ILanguageBinding CreateLanguageBinding(MockApiLanguage language);

        /// <summary>
        /// Creates the language validator.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        ILanguageValidator CreateLanguageValidator(MockApiLanguage language);
    }
}
