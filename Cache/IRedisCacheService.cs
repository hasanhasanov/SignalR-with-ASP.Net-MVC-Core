namespace chat.Cache
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRedisCacheService
    {
        /// <summary>
        /// Parametreden gelen anahtar ve satır anahtarına ilişkin yeni bir kayıt ekler
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task AddHashItemAsync<T>(string key, string rowKey, T value);

        /// <summary>
        /// Parametreden gelen anahtar ile ilgili kayıtların listesini çeker
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetHashItemsAsync<T>(string key);

        /// <summary>
        /// Parametreden gelen anahtar ve satırın anahtarı ile ilgili kaydı getirir
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetHashItemAsync<T>(string key, string rowKey);

        /// <summary>
        /// Parametreden gelen anahtara ve satır anahtarına ait olan kaydı siler
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        Task<bool> RemoveHashItemAsync(string key, string rowKey);
    }
}