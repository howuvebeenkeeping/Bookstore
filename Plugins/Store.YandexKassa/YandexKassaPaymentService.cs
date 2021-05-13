using System;
using System.Collections.Generic;
using Bookstore;
using Bookstore.Contractors;
using Store.Web.Contractors;

namespace Store.YandexKassa {
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService {
        public string UniqueCode => "YandexKassa";
        public string GetUri => "/YandexKassa/";
        public string Title => "Оплата по банковской карте";
        public Form CreateForm(Order order) {
            return new Form(UniqueCode, order.Id, 1, true, Array.Empty<Field>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values) {
            return new Form(UniqueCode, orderId, 2, true, Array.Empty<Field>());
        }

        public OrderPayment GetPayment(Form form) {
            return new OrderPayment(UniqueCode, "Оплата картой", new Dictionary<string, string>());
        }
    }
}