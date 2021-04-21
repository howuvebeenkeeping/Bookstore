using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Bookstore.Memory {
    public class BookRepository : IBookRepository {
        private readonly Book[] _books = {
            new(1, "ISBN 1231231231", "D. Knuth", "Art Of Programming", "Super classical book about programming", 12.45m),
            new(2, "ISBN 1231231232", "M. Fowler", "Refactoring", "Must read for refactoring code knowledge", 7.14m),
            new(3, "ISBN 1231231233", "B. Kernighan, D. Ritchie", "C Programming Language", "C forever", 10.99m)
        };

        public Book[] GetAllByIds(IEnumerable<int> bookIds) {
            return (from book in _books
                    join bookId in bookIds on book.Id equals bookId
                    select book).ToArray();
        }

        public Book[] GetAllByIsbn(string isbn) {
            return _books.Where(book => book.Isbn == isbn).ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string query) {
            return _books.Where(book => 
                    book.Author.ToLower().Contains(query.ToLower()) 
                    || book.Title.ToLower().Contains(query.ToLower()))
                    .ToArray();
        }

        public Book GetById(int id) {
            // если будет больше одного или ни одного совпадения, то будет выброшено исключение
            return _books.Single(book => book.Id == id);
        }
    }
}
