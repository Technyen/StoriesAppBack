using Api.Entities;
using Microsoft.Azure.Cosmos;
using User = Api.Entities.User;

namespace Api.Services
{
    public class CosmosService : IRepositoryService
    {
        private readonly CosmosClient _client;
        private readonly Database _database;
        private readonly Dictionary<string, string> _containerNames;

        public CosmosService(CosmosClient client)
        {
            _client = client;
            _database = _client.GetDatabase("cosmosapistories");
            _containerNames = new Dictionary<string, string>();
            _containerNames.Add(nameof(User), "users");
            _containerNames.Add(nameof(Story), "stories");
        }

        public async Task<T> CreateItemAsync<T>(T item)
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            return await container.CreateItemAsync(item);
        }

        public async Task<T?> UpdateItemAsync<T>(T item, string id)
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            return await container.ReplaceItemAsync(item, id);
        }

        public async Task<T?> FindItemAsync<T>(string propertyName, string? value)
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            var queryText = $"SELECT * FROM {_containerNames[typeof(T).Name]} p WHERE p.{propertyName} = '{value}'".ToLower();
            using var feedIterator = container.GetItemQueryIterator<T>(queryText);
            var items = await feedIterator.ReadNextAsync();
            return items.FirstOrDefault();
        }

        public async Task<List<T>> GetItemsAsync<T>()
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            var queryText = $"SELECT * FROM  {_containerNames[typeof(T).Name]}";
            using var feedIterator = container.GetItemQueryIterator<T>(queryText);
            var items = new List<T>();

            while (feedIterator.HasMoreResults)
            {
                var feedResponse = await feedIterator.ReadNextAsync();

                foreach (T item in feedResponse)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        public async Task<ItemResponse<T>> DeleteItemAsync<T>(string itemId, string partitionKey)
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            var response = await container.DeleteItemAsync<T>(itemId, new PartitionKey(partitionKey));
            return response;
        }
    }
}
