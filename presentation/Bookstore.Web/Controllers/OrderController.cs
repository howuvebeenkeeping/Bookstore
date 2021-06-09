﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bookstore.Contractors;
using Bookstore.Messages;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Contractors;

namespace Bookstore.Web.Controllers {
    public class OrderController : Controller {
        private readonly IBookRepository _bookRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;
        private readonly IEnumerable<IDeliveryService> _deliveryServices;
        private readonly IEnumerable<IPaymentService> _paymentServices;
        private readonly IEnumerable<IWebContractorService> _webContractorServices;

        public OrderController(
            IBookRepository bookRepository,
            IOrderRepository orderRepository, 
            INotificationService notificationService,
            IEnumerable<IDeliveryService> deliveryServices,
            IEnumerable<IPaymentService> paymentServices, IEnumerable<IWebContractorService> webContractorServices) {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _notificationService = notificationService;
            _deliveryServices = deliveryServices;
            _paymentServices = paymentServices;
            _webContractorServices = webContractorServices;
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1) {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            Book book = _bookRepository.GetById(bookId);
            
            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(bookId, book.Price, count);
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction(actionName: "Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult RemoveItem(int bookId) {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            
            order.Items.Remove(bookId);
            
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

            order.Items.Get(bookId).Count = count;
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction(actionName: "Index", "Order");
        }

        public IActionResult SendConfirmationCode(int id, string cellPhone) {
            Order order = _orderRepository.GetById(id);
            OrderModel model = Map(order);

            if (!IsValidCellPhone(cellPhone)) {
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";
                return View("Index", model);
            }

            var code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);
            _notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation", new ConfirmationModel() {
                CellPhone = cellPhone, OrderId = id
            });
        }

        private static bool IsValidCellPhone(string cellPhone) {
            if (cellPhone is null) {
                return false;
            }

            cellPhone = cellPhone
                .Replace(" ", "")
                .Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        [HttpPost]
        public IActionResult Confirm(int id, string cellPhone, int code) {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null) {
                return View("Confirmation",
                    new ConfirmationModel {
                        OrderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string> {
                            ["code"] = "Пустой код, повторите отправку"
                        }
                    });
            }

            if (storedCode != code) {
                return View("Confirmation",
                    new ConfirmationModel {
                        OrderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string> {
                            { "code", "Отличается от отправленного" }
                        }
                    });
            }

            Order order = _orderRepository.GetById(id);
            order.CellPhone = cellPhone;
            _orderRepository.Update(order);
            
            HttpContext.Session.Remove(cellPhone);

            var model = new DeliveryModel {
                OrderId = id,
                Methods = _deliveryServices.ToDictionary(service => service.UniqueCode,
                    service => service.Title)
            };

            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode) {
            IDeliveryService deliveryService = _deliveryServices
                .Single(service => service.UniqueCode == uniqueCode);
            
            Order order = _orderRepository.GetById(id);
            Form form = deliveryService.CreateForm(order);

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values) {
            IDeliveryService deliveryService = _deliveryServices
                .Single(service => service.UniqueCode == uniqueCode);
            
            Form form = deliveryService.MoveNextForm(id, step, values);

            if (form.IsFinal) {
                Order order = _orderRepository.GetById(id);
                order.Delivery = deliveryService.GetDelivery(form);
                _orderRepository.Update(order);

                var model = new DeliveryModel {
                    OrderId = id,
                    Methods = _paymentServices.ToDictionary(service => service.UniqueCode,
                        service => service.Title)
                };
                return View("PaymentMethod", model);
            }
            
            return View("DeliveryStep", form);
        }
        
        [HttpPost]
        public IActionResult StartPayment(int id, string uniqueCode) {
            IPaymentService paymentService = _paymentServices
                .Single(service => service.UniqueCode == uniqueCode);
            
            Order order = _orderRepository.GetById(id);
            Form form = paymentService.CreateForm(order);

            var webContractorService =
                _webContractorServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (webContractorService != null) {
                return Redirect(webContractorService.GetUri);
            }

            return View("PaymentStep", form);
        }

        [HttpPost]
        public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values) {
            IPaymentService paymentService = _paymentServices
                .Single(service => service.UniqueCode == uniqueCode);
            
            Form form = paymentService.MoveNextForm(id, step, values);

            if (form.IsFinal) {
                Order order = _orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                _orderRepository.Update(order);
                
                return View("Finish");
            }
            
            return View("PaymentStep", form);
        }

        public IActionResult Finish() {
            HttpContext.Session.RemoveCart();
            
            return View();
        }
    }
}
