namespace chat.Services.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using chat.Cache;
    using chat.Data.Entities;
    using chat.Data.Repositories;
    using chat.Services.Interfaces;

    /// <summary>
    ///* Kullanıcılar ile ilgili işlemler (CRUD) yapılır
    ///* İşlemlerin geneli Cache ve DB üzerinden yapılıyor
    ///! Bu gibi servisler genelde ayrı katmanda yer alırlar ve katmanın kendine özel DTO (modelleri) olur, işte Cache'te kullanılan modeller de onlar olmalı, sadece ihtiyaç duyulan alanlar olmalı içinde, Entity olmamalı
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRedisCacheService _cacheService;

        public UserService(
            IRepository<User> userRepository,
            IRedisCacheService cacheService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;

        }

        /// <summary>
        /// Kullanıcı listesini Cache'ten çeker
        /// Orada veri yok ise DB den çeker ve Cache'i güncelleyerek geri döner
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAsync()
        {
            var foo = _userRepository.GetAll().ToList();

            foreach (var item in foo)
            {
                await _cacheService.RemoveHashItemAsync(CacheKeys.USER, item.Name.ToUpper());
            }

            //* Cache'ten kullanıcı bilgilerini çek
            var users = await _cacheService.GetHashItemsAsync<User>(CacheKeys.USER);
            if (users != null && users.Any())
                return users;

            //* Eğer Cach'te yok ise DB den çek, Cache'e ata ve listeyi döndür
            users = _userRepository.GetAll().ToList();

            foreach (var item in users)
            {
                //* Cache'tetasarruf açısından sadece ihtiyaç duyulan alanları eklemek daha mantıklı
                await _cacheService.AddHashItemAsync($"{CacheKeys.USER}:{item.Id}", item.Name.ToUpper(), item);
            }

            return users;
        }

        /// <summary>
        /// Yeni kullanıcıyı DB'ye ve Cache'e ekler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentException("Name alanı zorunlu!");

            //* Mevcutta bu ver' var mi kontrol et
            var existsUser = _userRepository.FindOne(x => x.Name.ToLower() == user.Name.ToLower().Trim());
            if (existsUser != null)
                return;

            //* DB ye yen' kayit ekle
            await _userRepository.CreateAsync(user);

            //* Cache'e ekle
            await _cacheService.AddHashItemAsync($"{CacheKeys.USER}:{user.Id}", user.Name.ToUpper(), user);
        }

        /// <summary>
        /// Yeni kullanıcıyı DB'de ve Cache'te günceller
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentException("Name alanı zorunlu!");

            //* Güncellenen kaydın mevcut verilerde olup olmadığını kontrol et
            var existsUser = _userRepository.FindOne(x => x.Name.ToLower() == user.Name.ToLower().Trim());
            if (existsUser != null && existsUser.Id != user.Id)
                throw new ArgumentException("Bu isimde kayıt zaten var!");

            //* Mevcut veriyi DB de güncelle
            await _userRepository.UpdateAsync(user);

            //* Mevcut veriyi Cache'te güncelle
            await _cacheService.AddHashItemAsync($"{CacheKeys.USER}:{user.Id}", user.Name.ToUpper(), user);
        }

        /// <summary>
        /// İlgili kullanıcıyı hem DB den hemde Cache'ten siler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteAsync(User user)
        {
            if (user.Id <= 0)
                throw new ArgumentException("Id zorunlu!");

            var existsUser = await _cacheService.GetHashItemAsync<User>($"{CacheKeys.USER}:{user.Id}", user.Name.ToUpper().Trim());
            if (existsUser == null)
                throw new ArgumentException("Kullanıcı bulunamadı!");

            //* DB den ilgili kaydı sil
            await _userRepository.DeleteAsync(user);

            //* Cache'ten ilgili kaydı sil
            await _cacheService.RemoveHashItemAsync($"{CacheKeys.USER}:{user.Id}", user.Name.ToUpper());
        }
    }
}