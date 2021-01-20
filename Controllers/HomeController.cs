namespace chat.Controllers
{
    //* Namespace'lerin aşağıda olmasının nedeni kütüphane isimlerinin kısa yazılmasına olanak sağlar, böylece eğer büyük projeniz var ise boyutunu biraz olsa da indirmiş olursunuz ve ilk açıldığında çabuk açılmasına yardımcı olur (görülmeyecek kadar fark bunlar)
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using chat.Models;
    using chat.Data.Repositories;
    using chat.Data.Entities;
    using System.Linq;
    using chat.Models.Group;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Group> _groupRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IRepository<Group> groupRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
        }

        public IActionResult Index()
        {
            //* Tüm Grupları çek
            var groups = _groupRepository.GetAll();

            var result = new HomeViewModel();

            result.Groups = groups.Select(x => new GroupViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

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
