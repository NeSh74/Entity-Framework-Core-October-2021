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
            //int year = int.Parse(Console.ReadLine());
            string input = Console.ReadLine();
            string result = GetBookTitlesContaining(dbContex, input);

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
        //Books by Price 
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
        //Not Released In 
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();
            var books = context
                //var books = context
                //.Books
                //.AsEnumerable()
                //.Where(b => b.ReleaseDate.Value.Year != year)
                //.Select(b => new { b.Title, b.BookId })
                //.OrderBy(b => b.BookId)
                //.ToList();
                .Books
                .Where(b => b.ReleaseDate.HasValue &&
                            b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            //var sb = new StringBuilder();
            //books.ForEach(book => sb.AppendLine(book.Title));
            //var result = sb.ToString().TrimEnd();
            //return result;
            foreach (string title in books)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();

        }
        //Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var result = new StringBuilder();

            var bookTitles = context
                .BooksCategories
                .Where(bc => categories
                    .Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var title in bookTitles)
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }
        //Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateParts = date.Split("-");

            var day = int.Parse(dateParts[0]);
            var month = int.Parse(dateParts[1]);
            var year = int.Parse(dateParts[2]);

            var givenDate = new DateTime(year, month, day);

            var books = context
                .Books
                .Where(b => b.ReleaseDate < givenDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }
        //Author Search 
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] authorNames = context
                .Authors
                .ToArray()
                .Where(a => a.FirstName.ToLower().EndsWith(input.ToLower()))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(n => n)
                .ToArray();

            foreach (string authorName in authorNames)
            {
                sb.AppendLine(authorName);
            }

            return sb.ToString().TrimEnd();
        }
        //Book Search 
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context
                .Books
                .Where(b => b.Title.ToLower()
                    .Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            var result = new StringBuilder();

            foreach (var title in bookTitles)
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }
    }
}
