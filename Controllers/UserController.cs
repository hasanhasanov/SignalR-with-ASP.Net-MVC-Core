namespace Chat.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Chat.Models;
    using Chat.Services.Interfaces;
    using Chat.Models.User;
    using Chat.Data.Entities;
    using Chat.Helpers;

    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcı ekleme asyfası
        /// Projede default açılan ilk sayfa
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Yeni kullanıcı ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var entity = new User { Name = model.UserName };

                await _userService.CreateAsync(entity);

                return RedirectToAction("Index", "Home", new IndexViewModel { UserId = entity.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController.Create metodunda hata oluştu. Hata: {ex}", ex);
                TempData.AddError(ex.Message);
                return View();
            }
        }
    }
}
