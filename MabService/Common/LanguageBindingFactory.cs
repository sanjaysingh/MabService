using MabService.JsLanguageBinding;
using MabService.Shared;
using System;

namespace MabService.Common
{
    /// <summary>
    /// Language binding factory
    /// </summary>
    /// <seealso cref="MabService.Shared.ILanguageBindingFactory" />
    public class LanguageBindingFactory : ILanguageBindingFactory
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageBindingFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LanguageBindingFactory(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Creates the language binding.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public ILanguageBinding CreateLanguageBinding(MockApiLanguage language)
        {
            switch (language)
            {
                case MockApiLanguage.JavaScript:
                    return new JavaScriptLanguageBinding(logger);
            }

            return null;
        }

        /// <summary>
        /// Creates the language validator.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public ILanguageValidator CreateLanguageValidator(MockApiLanguage language)
        {
            switch (language)
            {
                case MockApiLanguage.JavaScript:
                    return new JavaScriptLanguageValidator();
            }

            return null;
        }
    }
}
