using Api.Entities;
using Microsoft.Azure.Cosmos;
using User = Api.Entities.User;

namespace Api.Services
{
    public class CosmosService : ICosmosService
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

        public async Task<T?> FindItemAsync<T>(string propertyName, string value)
        {
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            var queryString = $"SELECT * FROM {_containerNames[typeof(T).Name]} p WHERE p.{propertyName} = '{value}'".ToLower();
            using var filteredFeed = container.GetItemQueryIterator<T>(queryDefinition: new QueryDefinition(query: queryString));
            var items = await filteredFeed.ReadNextAsync();
            return items.FirstOrDefault();
        }

        public async Task<List<T>> GetItemsAsync<T>()
        {
            var items = new List<T>();
            var container = _database.GetContainer(_containerNames[typeof(T).Name]);
            using FeedIterator<T> feed = container.GetItemQueryIterator<T>(
                 queryText: $"SELECT * FROM  {_containerNames[typeof(T).Name]}"
                 );
            while (feed.HasMoreResults)
            {
                FeedResponse<T> response = await feed.ReadNextAsync();

                foreach (T item in response)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}