using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookstore {
    public class Book {
        public int Id { get; }
        public string Isbn { get; }
        public string Author { get; }
        public string Title { get; }
        public string Description { get; }
        public decimal Price { get; }

        public Book(int id, string isbn, string author, string title, string description, decimal price) {
            Id = id;
            Isbn = isbn;
            Author = author;
            Title = title;
            Description = description;
            Price = price;
        }

        public static bool IsIsbn(string line) {
            if (string.IsNullOrEmpty(line)) {
                return false;
            }

            line = line.Replace(" ", "")
                       .Replace("-", "")
                       .ToUpper();

            // ^ - точное начало строки, $ - точный конец строки, (\d{3})? - возможно еще 3 цифры
            return Regex.IsMatch(line, @"^ISBN\d{10}(\d{3})?$");
        }
     }
}
