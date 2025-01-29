namespace Identity.Interfaces
{
    public interface ICache
    {
        public Task<T> GetAsync<T>(string key);
        public Task<bool> SetAsync<T>(string key, T value);
        public Task RemoveAsync(string key);
    }
}
