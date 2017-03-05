using System;

namespace MabService.Shared
{
    public static class ResourceStrings
    {
        public static Func<string, string> CollectionCreated = (collectionName) => string.Format("Collection {0} created successfully.", collectionName);
    }
}
