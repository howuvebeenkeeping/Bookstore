using System;
using System.Linq;
using Xunit;

namespace Bookstore.Tests {
    public class OrderTests {
        [Fact]
        public void Order_WithNullItems_ThrowsArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => new Order(1, null));
        }

        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero() {
            var order = new Order(1, Array.Empty<OrderItem>());
            Assert.Equal(0, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero() {
            var order = new Order(1, Array.Empty<OrderItem>());
            Assert.Equal(0m, order.TotalPrice);
        }

        [Fact]
        public void TotalCount_WithNotEmptyItems_CalculatesTotalCount() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            decimal totalCount = order.Items.Sum(item => item.Count);

            Assert.Equal(totalCount, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithNotEmptyItems_CalculatesTotalPrice() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            decimal totalPrice = order.Items.Sum(item => item.Count * item.Price);

            Assert.Equal(totalPrice, order.TotalPrice);
        }
    }
}
