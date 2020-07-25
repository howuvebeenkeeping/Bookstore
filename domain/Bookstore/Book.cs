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
        public Book(int id, string isbn, string author, string title)
        {
            Title = title;
            Id = id;
            Isbn = isbn;
            Author = author;
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
