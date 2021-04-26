using System;
using System.Collections.Generic;

namespace Bookstore.Contractors {
    public class CashPaymentService : IPaymentService {
        public string UniqueCode => "Cash";
        public string Title => "Оплата наличными";
        
        public Form CreateForm(Order order) {
            return new(UniqueCode, order.Id, 1, false, Array.Empty<Field>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values) {
            if (step != 1) {
                throw new InvalidOperationException("Invalid cash step");
            }

            return new Form(UniqueCode, orderId, 2, true, Array.Empty<Field>());
        }

        public OrderPayment GetPayment(Form form) {
            if (form.UniqueCode != UniqueCode || !form.IsFinal) {
                throw new InvalidOperationException("Invalid payment form");
            }

            return new OrderPayment(UniqueCode, "Оплата наличными", new Dictionary<string, string>());
        }
    }
}