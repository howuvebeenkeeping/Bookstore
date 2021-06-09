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
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5)
            });

            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithNotEmptyItems_CalculatesTotalPrice() {
            var order = new Order(1, new[] {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5)
            });

            Assert.Equal(3 * 10m + 5 * 100m, order.TotalPrice);
        }
    }
}