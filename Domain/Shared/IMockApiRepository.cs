namespace MabService.Domain.Shared
{
    /// <summary>
    /// A mock API data repository contract
    /// </summary>
    public interface IMockApiRepository
    {
        MockApiCollection GetCollection(string id);

        bool CollectionExists(string collectionId);

        MockApiModel AddMockApi(MockApiModel mockApi);
    }
}
