using Microsoft.AspNetCore.Mvc;
using Bookstore.YandexKassa.Areas.YandexKassa.Models;

namespace Bookstore.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index(int orderId, string returnUri)
        {
            var model = new ExampleModel
            {
                OrderId = orderId,
                ReturnUri = returnUri,
            };

            return View(model);
        }

        public IActionResult Callback(int orderId, string returnUri)
        {
            var model = new ExampleModel
            {
                OrderId = orderId,
                ReturnUri = returnUri,
            };

            return View(model);
        }
    }
}