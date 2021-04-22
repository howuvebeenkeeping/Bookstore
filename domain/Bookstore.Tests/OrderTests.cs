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

            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithNotEmptyItems_CalculatesTotalPrice() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            Assert.Equal(3 * 10m + 5 * 100m, order.TotalPrice);
        }

        [Fact]
        public void AddOrUpdateItem_WithNullBook_ThrowsArgumentNullException() {
            var order = new Order(1, Array.Empty<OrderItem>());

            Assert.Throws<ArgumentNullException>(() => order.AddOrUpdateItem(null, 0));
        }

        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });
            
            var book = new Book(1, null, null, null, null, 0m);
            order.AddOrUpdateItem(book, 4);

            Assert.Equal(3 + 4, order.GetItem(1).Count);
        }
        
        [Fact]
        public void AddOrUpdateItem_WithNonExistingItem_AddsCount() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });
            
            var book = new Book(3, null, null, null, null, 0m);
            order.AddOrUpdateItem(book, 2);

            Assert.Equal(2, order.GetItem(3).Count);
        }

        [Fact]
        public void GetItem_WithExistingItem_ReturnsItem() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            OrderItem orderItem = order.GetItem(1);
            
            Assert.Equal(3, orderItem.Count);
        }
        
        [Fact]
        public void GetItem_WithNonExistingItem_ThrowsInvalidOperationException() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            Assert.Throws<InvalidOperationException>(() => {
                order.GetItem(3);
            });
        }
        
        [Fact]
        public void RemoveItem_WithExistingItem_RemovesItem() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });
            
            order.RemoveItem(1);

            Assert.Equal(1, order.Items.Count);
        }
        
        [Fact]
        public void RemoveItem_WithNonExistingItem_ThrowsInvalidOperationException() {
            var order = new Order(1, new[] {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            Assert.Throws<InvalidOperationException>(() => {
                order.RemoveItem(3);
            });
        }
    }
}
