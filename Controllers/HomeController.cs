namespace Chat.Controllers
{
    //* Namespace'lerin aşağıda olmasının nedeni kütüphane isimlerinin kısa yazılmasına olanak sağlar, böylece eğer büyük projeniz var ise boyutunu biraz olsa da indirmiş olursunuz ve ilk açıldığında çabuk açılmasına yardımcı olur (görülmeyecek kadar fark bunlar)
    using System.Diagnostics;
    using System.Linq;
    using Data.Entities;
    using Data.Repositories;
    using Models;
    using Models.Group;
    using Models.User;
    using Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Group> _groupRepository;
        private readonly IUserService _userservice;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService userservice,
            IRepository<Group> groupRepository)
        {
            _logger = logger;
            _userservice = userservice;
            _groupRepository = groupRepository;
        }

        public IActionResult Index(IndexViewModel model)
        {
            //* Tüm Grupları çek
            var groups = _groupRepository.GetAll();

            //* Tüm Kullanıcıları çek
            var users = _userservice.GetAsync();

            //* Response modeli oluştur
            var result = new IndexViewModel
            {
                Groups = groups.Select(x => new GroupViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList(),

                Users = groups.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.Name
                }).ToList()
            };

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
