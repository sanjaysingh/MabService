using System;

namespace MabService.Shared
{
    public static class Constants
    {
        public const int MaxNameLength = 30;

        public const int MinApiBodyLength = 1;

        public const int MaxApiBodyLength = 2048;

        public const int MinNameLength = 1;

        public const int MinApiTemplateLength = 1;

        public const int MaxApiTemplateLength = 100;

        public static Func<string, string> CollectionCreatedMessage = (collectionName) => string.Format("Collection {0} created successfully.", collectionName);

        public static readonly string InvalidCollectionNameMessage = $"Collection name should be alphanumeric and less than {MaxNameLength} characters.";

        public static readonly string InvalidMockApiNameMessage = $"Api name should be alphanumeric and less than {MaxNameLength} characters.";

        public const string ResourceNotFoundMessage = "The asked resource was not found. Check your route and try again.";

        public const string ResourceConflictMessage = "Resource already exists.";

        public const string MethodNotAllowedExceptionMessage = "The specified http method is not allowed for this resource.";

        public const string InvalidApiVerbMessage = "The http verb for the API is either unspecified or invalid.";

        public static readonly string InvalidApiTempateMessage = $"Invalid api template. Ensure that it is less than {MaxApiTemplateLength} characters, does not contain spaces and conforms to template signature.";

        public static Func<string,string> InvalidApiBodyMessage = (body) => $"Invalid api function definition. " +
                        $"Ensure that your api function conforms to a valid Java Script function requirement. " +
                        "Api body should be in the format \"function run (req, res) { <<Your code goes here>> }\"" +
                        $"Body received = {body}";

        public const string InternalServerErrorMessage = "There was an unhandled error processing your request.";

        public const string InternalServerErrorId = "InternalServerError.";
    }
}
