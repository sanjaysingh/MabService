using System;

namespace MabService.Shared
{
    public static class ResourceStrings
    {
        public static Func<string, string> CollectionCreated = (collectionName) => string.Format("Collection {0} created successfully.", collectionName);

        public const string InvalidCollectionName = "Collection name should be alphanumeric and less than 20 characters.";

        public const string ResourceNotFound = "The asked resource was not found. Check your route and try again.";

        public const string InternalServerErrorMessage = "There was an unhandled error processing your request.";
        public const string InternalServerErrorId = "InternalServerError.";
    }
}
