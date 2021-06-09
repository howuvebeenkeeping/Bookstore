using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bookstore.Tests {
    public class OrderItemsTests {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 0m, 0));
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 0m, -1));
        }

        [Fact]
        public void OrderItem_WithPositiveCount_ThrowsArgumentOutOfRangeException() {
            var orderItem = new OrderItem(1, 3m, 2);
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOutOfRangeException() {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                orderItem.Count = -1;
            });
        }
        
        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOutOfRangeException() {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                orderItem.Count = 0;
            });
        }
        
        [Fact]
        public void Count_WithPositiveValue_SetsValue() {
            var orderItem = new OrderItem(0, 0m, 5) {
                Count = 10
            };

            Assert.Equal(10, orderItem.Count);
        }
    }
}
