using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bookstore.Tests {
    public class OrderItemsTests {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 0, 0m));
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 0, 0m));
        }

        [Fact]
        public void OrderItem_WithPositiveCount_ThrowsArgumentOutOfRangeException() {
            var orderItem = new OrderItem(1, 2, 3m);
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
        }
    }
}
