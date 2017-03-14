using MabService.Shared;
using MabService.Shared.Exceptions;
using System;

namespace MabService.JsLanguageBinding
{
    /// <summary>
    /// Javascript language validator
    /// </summary>
    /// <seealso cref="MabService.JsLanguageBinding.ILanguageValidator" />
    public class JavaScriptLanguageValidator : ILanguageValidator
    {
        /// <summary>
        /// Validates the specified API model.
        /// </summary>
        /// <param name="apiSource">The API source.</param>
        /// <exception cref="ValidationException"></exception>
        public void Validate(string apiSource)
        {
            if (apiSource.IsNullOrWhiteSpace() ||
                apiSource.IsNotInLength(Constants.MinApiBodyLength, Constants.MaxApiBodyLength) ||
                (!apiSource.Trim().Replace(" ", "").StartsWith("functionrun(req,res){")) ||
                (!apiSource.EndsWith("}")))
            {
                throw new ValidationException(Constants.InvalidApiBodyMessage);
            }

            try
            {
                JavaScriptRuntime.Compile(apiSource);
            }
            catch(Exception ex)
            {
                throw new ValidationException(Constants.InvalidApiBodyMessage, ex);
            }
        }
    }
}
