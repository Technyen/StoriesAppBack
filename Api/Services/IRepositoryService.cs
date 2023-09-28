using Microsoft.Azure.Cosmos;


namespace Api.Services

{
    public interface IRepositoryService
    {
        Task<T> CreateItemAsync<T>(T item);
        Task<T?> FindItemAsync<T>(string propertyName, string? value);
        Task<List<T>> GetItemsAsync<T>();
        Task<T?> UpdateItemAsync<T>(T item, string id);
        Task<ItemResponse<T>> DeleteItemAsync<T>(string storyId, string partitionKey);
    }
}