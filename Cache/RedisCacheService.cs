namespace chat.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IOptions<RedisCacheSettings> _options;
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            _options = options;
        }

        /// <summary>
        /// Parametreden gelen anahtar ve satır anahtarına ilişkin yeni bir kayıt atar
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task AddHashItemAsync<T>(string key, string rowKey, T value)
        {
            DoConnect();

            var database = _lazyConnection.Value.GetDatabase(db: _options.Value.Db);

            await database.HashSetAsync(key, new HashEntry[] { new HashEntry(rowKey, JsonConvert.SerializeObject(value)) });
        }

        /// <summary>
        /// Parametreden gelen anahtar ve satırın anahtarı ile ilgili kaydı getirir
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetHashItemAsync<T>(string key, string rowKey)
        {
            DoConnect();

            var database = _lazyConnection.Value.GetDatabase(db: _options.Value.Db);

            var result = await database.HashGetAsync(key, rowKey);

            return !result.IsNull ? JsonConvert.DeserializeObject<T>(result) : default(T);
        }

        /// <summary>
        /// Parametreden gelen anahtar ile ilgili kayıtların listesini çeker
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetHashItemsAsync<T>(string key)
        {
            DoConnect();

            var result = new List<T>();

            var database = _lazyConnection.Value.GetDatabase(db: _options.Value.Db);

            var cacheValue = await database.HashGetAllAsync(key);

            foreach (var item in cacheValue)
            {
                result.Add(JsonConvert.DeserializeObject<T>(item.Value));
            }

            return result;
        }

        /// <summary>
        /// Parametreden gelen anahtara ve satır anahtarına ait olan kaydı siler
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<bool> RemoveHashItemAsync(string key, string rowKey)
        {
            DoConnect();

            var database = _lazyConnection.Value.GetDatabase(db: _options.Value.Db);

            bool result = true;
            if (await database.KeyExistsAsync(key))
                result = await database.HashDeleteAsync(key, rowKey);

            return result;
        }

        #region Helpers

        /// <summary>
        /// Connection açık değilse yeni connection açar
        /// </summary>
        private void DoConnect()
        {
            if (_lazyConnection == null || !_lazyConnection.Value.IsConnected)
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { { _options.Value.Master } },
                    AllowAdmin = true,
                    AbortOnConnectFail = false,
                    Ssl = false
                };

                _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
            }
        }

        #endregion
    }
}