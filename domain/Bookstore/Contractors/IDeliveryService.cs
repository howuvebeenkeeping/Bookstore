using System.Collections.Generic;

namespace Bookstore.Contractors {
    public interface IDeliveryService {
        string Name { get; }
        string Title { get; }
        Form FirstForm(Order order);
        Form NextForm(int step, IReadOnlyDictionary<string, string> values);
        OrderDelivery GetDelivery(Form form);
    }
}