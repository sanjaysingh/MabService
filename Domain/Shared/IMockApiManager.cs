using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabService.Domain.Shared
{
    /// <summary>
    /// Mock APIs manager contract
    /// </summary>
    public interface IMockApiManager
    {
        Task CreateCollectionAsync(string collectionName);
    }
}
