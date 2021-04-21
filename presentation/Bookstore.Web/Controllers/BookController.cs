﻿using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers {
    public class BookController : Controller {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }
        public IActionResult Index(int id) {
            Book book = _bookRepository.GetById(id);

            return View(book);
        }
    }
}
