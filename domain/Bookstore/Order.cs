using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bookstore
{
    public class Order
    {
        private List<OrderItem> items;
        public int Id { get; }
        public IReadOnlyCollection<OrderItem> Items => items;
        public int TotalCount => items.Sum(x => x.Count);
        public decimal TotalPrice => items.Sum(x => x.Price * x.Count);

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            this.items = new List<OrderItem>(items);
        }

        // Добавить тесты
        public void AddItem(Book book, int count)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            var item = items.SingleOrDefault(x => x.BookId == book.Id);

            if (item is null)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(book.Id, item.Count + count, book.Price));
            }

        }
    }
}
