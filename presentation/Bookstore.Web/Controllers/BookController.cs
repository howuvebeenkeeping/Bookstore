using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.App;

namespace Bookstore.Web.Controllers {
    public class BookController : Controller {
        private readonly BookService _bookService;

        public BookController(BookService bookService) {
            _bookService = bookService;
        }
        public IActionResult Index(int id) {
            var model = _bookService.GetById(id);

            return View(model);
        }
    }
}
