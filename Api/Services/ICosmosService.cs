using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using Api.Entities;


namespace Api.Services

{
    public interface ICosmosService
    {
        Task<T> CreateItemAsync<T>(T item);
        Task<T?> FindItemAsync<T>(string propertyName, string? value);
        Task<List<T>> GetItemsAsync<T>();
        Task<T?> UpdateItemAsync<T>(T item, string id);
        Task<ItemResponse<T>> DeleteItemAsync<T>(string storyId, string partitionKey);
    }
}