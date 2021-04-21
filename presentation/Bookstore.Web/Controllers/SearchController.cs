using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookService _bookService;

        public SearchController(BookService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Index(string query)
        {
            Book[] books = _bookService.GetAllByQuery(query);
            return View("Index", books);
        }
    }
}
