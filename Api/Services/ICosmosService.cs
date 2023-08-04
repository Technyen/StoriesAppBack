namespace Api.Services
{
    public interface ICosmosService
    {
        Task<T> CreateItemAsync<T>(T user);
        Task<T> FindItemAsync<T>(string value, string property);
        Task<List<T>> GetItemsAsync<T>();
    }
}