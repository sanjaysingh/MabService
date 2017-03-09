using MabService.Shared;
using MabService.Shared.Exceptions;
using System.Collections.Generic;

namespace MabService.Common
{
    /// <summary>
    /// A class for holding all the validation methods
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates the name of the collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateCollectionName(string collectionName)
        {
            if (IsNameInvalid(collectionName))
            {
                throw new ValidationException(Constants.InvalidCollectionNameMessage);
            }
        }

        /// <summary>
        /// Validates the mock API resource.
        /// </summary>
        /// <param name="mockApi">The mock API.</param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateMockApiResource(MockApiResourceModel mockApi)
        {
            List<string> errors = new List<string>();

            // verify name
            if (IsNameInvalid(mockApi.Name))
            {
                errors.Add(Constants.InvalidMockApiNameMessage);
            }

            // verify verb
            if(mockApi.Verb == MockApiHttpVerb.None)
            {
                errors.Add(Constants.InvalidApiVerbMessage);
            }

            // verify body
            if(mockApi.Body.IsNullOrWhiteSpace() || 
                mockApi.Body.IsNotInLength(Constants.MinApiBodyLength, Constants.MaxApiBodyLength))
            {
                errors.Add(Constants.InvalidApiBodyMessage);
            }

            // verify route template
            if (mockApi.RouteTemplate.IsNullOrWhiteSpace() || 
                mockApi.RouteTemplate.IsNotInLength(Constants.MinApiTemplateLength, Constants.MaxApiTemplateLength))
            {
                errors.Add(Constants.InvalidApiTempateMessage);
            }

            if(errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }

        /// <summary>
        /// Determines whether [is name invalid] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is name invalid] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNameInvalid(string name)
        {
            return name.IsNullOrWhiteSpace() || 
                    name.IsNotAlphanumeric() || 
                    name.IsNotInLength(Constants.MinNameLength, Constants.MaxNameLength);
        }
    }
}
