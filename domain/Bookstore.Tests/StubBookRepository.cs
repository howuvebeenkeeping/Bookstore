using System;
using System.Collections.Generic;

namespace Bookstore.Tests {
    internal class StubBookRepository : IBookRepository {
        public Book[] ResultOfGetAllByIsbn { get; init; }
        public Book[] ResultOfGetAllByTitleOrAuthor { get; init; }

        public Book[] GetAllByIsbn(string isbn) => ResultOfGetAllByIsbn;

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor) => ResultOfGetAllByTitleOrAuthor;

        public Book GetById(int id) => throw new NotImplementedException();

        public Book[] GetAllByIds(IEnumerable<int> bookIds) => throw new NotImplementedException();
    }
}
