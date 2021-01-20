namespace chat.SignalR
{
    using System;
    using System.Threading.Tasks;
    using chat.Cache;
    using chat.Services.Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class ChatHub : Hub
    {
        private readonly IRedisCacheService _cacheService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(
            IRedisCacheService cacheService,
            ILogger<ChatHub> logger,
            IUserService userService,
            IGroupService groupService)
        {
            _cacheService = cacheService;
            _logger = logger;
            _userService = userService;
            _groupService = groupService;
        }

        /// <summary>
        /// Kişilere mesaj yollar
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Gruba katılma işlemleri
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public async Task JoinRoom(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.OthersInGroup(groupName).SendAsync(Context.User.Identity.Name + " joined.");
        }

        /// <summary>
        /// Gruptançıkma işlemleri
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.OthersInGroup(roomName).SendAsync(Context.User.Identity.Name + " leaved.");
        }

        /// <summary>
        /// Hub Connection açıldığında tetiklenir
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Connection açıldı!");

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Hub Connection kapandığında tetiklenir
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogWarning("Connection kapandı!");
            await base.OnDisconnectedAsync(exception);
        }


        #region Helpers


        #endregion
    }
}