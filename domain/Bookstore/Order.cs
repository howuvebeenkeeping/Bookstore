using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bookstore {
    public class Order {
        public int Id { get; }
        private readonly List<OrderItem> _items;
        public IReadOnlyCollection<OrderItem> Items => _items;
        public int TotalCount => _items.Sum(x => x.Count);
        public decimal TotalPrice => _items.Sum(x => x.Price * x.Count);

        public Order(int id, IEnumerable<OrderItem> items) {
            if (items == null) {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;
            _items = new List<OrderItem>(items);
        }

        public OrderItem GetItem(int bookId) {
            int index = _items.FindIndex(item => item.BookId == bookId);

            if (index == -1) {
                ThrowBookException("Book not found.", bookId);
            }

            return _items[index];
        }

        // TODO: Добавить тесты
        public void AddOrUpdateItem(Book book, int count) {
            if (book is null) {
                throw new ArgumentNullException(nameof(book));
            }

            int index = _items.FindIndex(item => item.BookId == book.Id);
            if (index == -1) {
                _items.Add(new OrderItem(book.Id, count, book.Price));
            } else {
                _items[index].Count += count;
            }
        }

        public void RemoveItem(int bookId) {
            int index = _items.FindIndex(x => x.BookId == bookId);

            if (index == -1) {
                ThrowBookException("Order doesn't contain specified item.", bookId);
            }
            
            _items.RemoveAt(index);
        }

        private void ThrowBookException(string message, int bookId) {
            var exception = new InvalidOperationException(message);
            exception.Data[nameof(bookId)] = bookId;

            throw exception;
        }
    }
}
