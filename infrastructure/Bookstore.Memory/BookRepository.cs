using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Bookstore.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = 
        {
            new Book(1, "ISBN 1231231231", "D. Knuth", "Art Of Programming", "Super classical book about programming", 12.45m),
            new Book(2, "ISBN 1231231232", "M. Fowler", "Refactoring", "Must read for refactoring code knowledge", 7.14m),
            new Book(3, "ISBN 1231231233", "B. Kernighan, D. Ritchie", "C Programming Language", "C forever", 10.99m)
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn).ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string query)
        {
            return books.Where(book => book.Author.ToLower().Contains(query.ToLower())
                                    || book.Title.ToLower().Contains(query.ToLower()))
                        .ToArray();
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
