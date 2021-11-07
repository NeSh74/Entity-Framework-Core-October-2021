using System;
using System.Linq;
using System.Text;
using BookShop.Models.Enums;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        private const int MinPrice = 40;

        public static void Main()
        {
            using var dbContex = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContex);

            //string ageRestrictionString = Console.ReadLine();
            string result = GetBooksByPrice(dbContex);

            Console.WriteLine(result);
        }
        //Age Restriction 
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] bookTitles = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (string title in bookTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
        //Golden Books 
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            string[] goldenBooksTitle = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (string title in goldenBooksTitle)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            var sb = new StringBuilder();
            books.ForEach(book => sb.AppendLine($"{book.Title} - ${book.Price:F2}"));
            var result = sb.ToString().TrimEnd();
            return result;
        }
    }
}
