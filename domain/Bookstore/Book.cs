using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookstore
{
    public class Book
    {
        public int Id { get; }
        public string Isbn { get; }
        public string Author { get; }
        public string Title { get; }
        public string Description { get; }
        public decimal Price { get; }

        public Book(int id, string isbn, string author, string title, string description, decimal price)
        {
            Title = title;
            Id = id;
            Isbn = isbn;
            Author = author;
            Description = description;
            Price = price;
        }

        internal static bool IsIsbn(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            str = str.Replace(" ", "")
                     .Replace("-", "")
                     .ToUpper();

            return Regex.IsMatch(str, @"^ISBN\d{10}(\d{3})?$");
        }
    }
}
