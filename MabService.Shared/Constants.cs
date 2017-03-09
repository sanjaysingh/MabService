using System;

namespace MabService.Shared
{
    public static class Constants
    {
        public const int MaxNameLength = 30;

        public const int MinApiBodyLength = 1;

        public const int MaxApiBodyLength = 200;

        public const int MinNameLength = 1;

        public const int MinApiTemplateLength = 1;

        public const int MaxApiTemplateLength = 100;

        public static Func<string, string> CollectionCreatedMessage = (collectionName) => string.Format("Collection {0} created successfully.", collectionName);

        public static readonly string InvalidCollectionNameMessage = $"Collection name should be alphanumeric and less than {MaxNameLength} characters.";

        public static readonly string InvalidMockApiNameMessage = $"Api name should be alphanumeric and less than {MaxNameLength} characters.";

        public const string ResourceNotFoundMessage = "The asked resource was not found. Check your route and try again.";

        public const string InvalidApiVerbMessage = "The http verb for the API is either unspecified or invalid.";

        public static readonly string InvalidApiTempateMessage = $"Invalid api template. Ensure that it is less than {MaxApiTemplateLength} characters, does not contain spaces and conforms to template signature.";

        public const string InvalidApiBodyMessage = "Invalid api function definition. Ensure that your api function conforms to a valid Java Script function requirement.";

        public const string InternalServerErrorMessage = "There was an unhandled error processing your request.";

        public const string InternalServerErrorId = "InternalServerError.";
    }
}
