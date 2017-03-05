using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabService.Domain.Shared
{
    public static class ResourceStrings
    {
        public static Func<string, string> CollectionCreated = (collectionName) => string.Format("Collection {0} created successfully.", collectionName);
    }
}
