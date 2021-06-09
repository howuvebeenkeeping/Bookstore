using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bookstore {
    public class Order {
        public int Id { get; }
        public OrderItemCollection Items { get; }
        public string CellPhone { get; set; }
        public OrderDelivery Delivery { get; set; }
        public OrderPayment Payment { get; set; }
        public int TotalCount => Items.Sum(x => x.Count);

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Count)
                                     + (Delivery?.Amount ?? 0m);

        public Order(int id, IEnumerable<OrderItem> items) {
            if (items == null) {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;
            Items = new OrderItemCollection(items);
        }
    }
}
