namespace Bookstore.Web.Models {
    // Data transfer object - хранит только свойства
    public class OrderItemModel {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
