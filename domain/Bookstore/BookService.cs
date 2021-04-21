using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Bookstore {
    public class BookService {
        private readonly IBookRepository _bookRepository;
        
        // внедрение зависемостей через конструктор
        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public Book[] GetAllByQuery(string query) {
            return Book.IsIsbn(query)
                    ? _bookRepository.GetAllByIsbn(query)
                    : _bookRepository.GetAllByTitleOrAuthor(query);
        }
    }
}
