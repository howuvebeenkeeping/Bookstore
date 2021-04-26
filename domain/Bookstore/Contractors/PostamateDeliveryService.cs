using System.Collections.Generic;
using System;
using System.Linq;

namespace Bookstore.Contractors {
    public class PostamateDeliveryService : IDeliveryService {
        public string UniqueCode => "Postamate";
        public string Title => "Доставка через постаматы в Москве и Санкт-Петербурге";

        private static readonly IReadOnlyDictionary<string, string> Cities = new Dictionary<string, string>() {
            ["1"] = "Москва",
            ["2"] = "Санкт-Петербург"
        };

        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Postamates =
            new Dictionary<string, IReadOnlyDictionary<string, string>> {
                ["1"] = new Dictionary<string, string> {
                    ["1"] = "Казанский вокзал",
                    ["2"] = "Охотный ряд",
                    ["3"] = "Савеловский рынок"
                },
                ["2"] = new Dictionary<string, string> {
                    ["4"] = "Московский вокзал",
                    ["5"] = "Гостиный двор",
                    ["6"] = "Петропавловская крепость"
                }
            };
        
        public Form CreateForm(Order order) {
            if (order is null) {
                throw new ArgumentNullException(nameof(order));
            }

            return new Form(UniqueCode, order.Id, 1, false, new[] {
                new SelectionField("Город", "city", "1", Cities)
            });
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values) {
            return step switch {
                1 => values["city"] switch {
                    "1" => new Form(UniqueCode, orderId, 2, false,
                        new Field[] {
                            new HiddenField("Город", "city", "1"),
                            new SelectionField("Постамат", "postamate", "1", Postamates["1"]),
                        }),
                    "2" => new Form(UniqueCode, orderId, 2, false,
                        new Field[] {
                            new HiddenField("Город", "city", "2"),
                            new SelectionField("Постамат", "postamate", "4", Postamates["2"]),
                        }),
                    _ => throw new InvalidOperationException("Invalid postamate city.")
                },
                2 => new Form(UniqueCode, orderId, 3, true,
                    new Field[] {
                        new HiddenField("Город", "city", values["city"]),
                        new HiddenField("Постамат", "postamate", values["postamate"]),
                    }),
                _ => throw new InvalidOperationException("Invalid postamate step.")
            };
        }

        public OrderDelivery GetDelivery(Form form) {
            if (form.UniqueCode != UniqueCode || !form.IsFinal) {
                throw new InvalidOperationException("Invalid form");
            }

            string cityId = form.Fields
                                .Single(field => field.Name == "city")
                                .Value;
            
            string cityName = Cities[cityId];
            
            string postamateId = form.Fields
                                     .Single(field => field.Name == "postamate")
                                     .Value;
            
            string postamateName = Postamates[cityId][postamateId];

            var parameters = new Dictionary<string, string> {
                [nameof(cityId)]        =  cityId,
                [nameof(cityName)]      =  cityName,
                [nameof(postamateId)]   =  postamateId,
                [nameof(postamateName)] =  postamateName,
            };

            var description = $"Город: {cityName}\nПостамат: {postamateName}";

            return new OrderDelivery(UniqueCode, description, 150m, parameters);
        }
    }
}