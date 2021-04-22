using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers {
    public class OrderController : Controller {
        private readonly IBookRepository _bookRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository) =>
            (_bookRepository, _orderRepository) = (bookRepository, orderRepository);
        

        public IActionResult Index() {
            if (!HttpContext.Session.TryGetCart(out Cart cart)) { return View("Empty"); }
            
            Order order = _orderRepository.GetById(cart.OrderId);
            OrderModel model = Map(order);

            return View(model);
        }

        private OrderModel Map(Order order) {
            IEnumerable<int> bookIds = order.Items.Select(item => item.BookId);
            Book[] books = _bookRepository.GetAllByIds(bookIds);
            IEnumerable<OrderItemModel> itemModels = 
                from item in order.Items
                join book in books on item.BookId equals book.Id
                select new OrderItemModel {
                    BookId = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Price = item.Price,
                    Count = item.Count
                };

            return new OrderModel {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice
            };
        }

        public IActionResult AddItem(int bookId, int count = 1) {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var book = _bookRepository.GetById(bookId);
            
            order.AddOrUpdateItem(book, count);
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }


        public IActionResult RemoveItem(int bookId) {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            
            order.RemoveItem(bookId);
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction(actionName: "Index", "Order");
        }

        private void SaveOrderAndCart(Order order, Cart cart) {
            _orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);
        }

        private (Order order, Cart cart) GetOrCreateOrderAndCart() {
            Order order;

            if (HttpContext.Session.TryGetCart(out Cart cart)) {
                order = _orderRepository.GetById(cart.OrderId);
            } else {
                order = _orderRepository.Create();
                cart = new Cart(order.Id);
            }

            return (order, cart);
        }
        
        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count) {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.GetItem(bookId).Count = count;
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction(actionName: "Index", "Order");
        }
    }
}
