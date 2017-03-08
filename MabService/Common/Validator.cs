using MabService.Shared;
using MabService.Shared.Exceptions;

namespace MabService.Common
{
    public static class Validator
    {
        public static void ValidateCollectionName(string collectionName)
        {
            if (collectionName.IsNullOrWhiteSpace() || 
                (!collectionName.IsAlphanumeric() ||
                (!collectionName.IsInLength(1, 20))))
            {
                throw new ValidationException(ResourceStrings.InvalidCollectionName);
            }
        }
    }
}
