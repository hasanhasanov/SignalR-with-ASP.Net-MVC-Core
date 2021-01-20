namespace Chat.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Chat.Data.Entities;

    public interface IUserService
    {
        /// <summary>
        /// Kullanıcı listesini Cache'ten çeker
        /// Orada veri yok ise DB den çeker ve Cache'i güncelleyerek geri döner
        /// </summary>
        /// <returns>Kullanıcıların listesini</returns>
        Task<List<User>> GetAsync();

        /// <summary>
        /// Yeni kullanıcıyı DB'ye ve Cache'e ekler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateAsync(User user);

        /// <summary>
        /// Yeni kullanıcıyı DB'de ve Cache'te günceller
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// İlgili kullanıcıyı hem DB den hemde Cache'ten siler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeleteAsync(User user);
    }
}