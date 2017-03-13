using MabService.Shared;
namespace MabService.Shared
{
    /// <summary>
    /// Language validator contract
    /// </summary>
    public interface ILanguageValidator
    {
        /// <summary>
        /// Validates the specified API model.
        /// </summary>
        /// <param name="apiSource">The API source.</param>
        void Validate(string apiSource);
    }
}
