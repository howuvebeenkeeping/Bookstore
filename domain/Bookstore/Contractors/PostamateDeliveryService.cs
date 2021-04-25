using System.Collections.Generic;
using System;

namespace Bookstore.Contractors {
    public class PostamateDeliveryService : IDeliveryService {
        public string UniqueCode => "Postamate";
        public string Title => "Доставка через постаматы";

        private static IReadOnlyDictionary<string, string> _cities = new Dictionary<string, string>() {
            ["1"] = "Москва",
            ["2"] = "Санкт-Петербург"
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates =
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
                throw new ArgumentNullException();
            }

            return new Form(UniqueCode, order.Id, 1, false, new[] {
                new SelectionField("Город", "city", "1", _cities)
            });
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values) {
            return step switch {
                1 => values["city"] switch {
                    "1" => new Form(UniqueCode, orderId, 1, false,
                        new Field[] {
                            new HiddenField("Город", "city", "1"),
                            new SelectionField("Постамат", "postamate", "1", postamates["1"]),
                        }),
                    "2" => new Form(UniqueCode, orderId, 2, false,
                        new Field[] {
                            new HiddenField("Город", "city", "2"),
                            new SelectionField("Постамат", "postamate", "4", postamates["2"]),
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
    }
}