using Bookstore.Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace Bookstore.Web {
    public static class SessionExtensions {
        private static readonly string Key = "Cart";

        public static void RemoveCart(this ISession session) {
            session.Remove(Key);
        }
        public static void Set(this ISession session, Cart cart) {
            if (cart == null) { return; }

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            
            writer.Write(cart.OrderId);
            writer.Write(cart.TotalCount);
            writer.Write(cart.TotalPrice);

            session.Set(Key, stream.ToArray());
        }

        public static bool TryGetCart(this ISession session, out Cart cart) { 
            if (session.TryGetValue(Key, out byte[] buffer)) {
                using var stream = new MemoryStream(buffer);
                using var reader = new BinaryReader(stream, Encoding.UTF8, true);
                
                int orderId = reader.ReadInt32();
                int totalCount = reader.ReadInt32();
                decimal totalPrice = reader.ReadDecimal();

                cart = new Cart(orderId) {
                    TotalCount = totalCount,
                    TotalPrice = totalPrice
                };

                return true;
            }
            cart = null;
            return false;
        }
    }
}
