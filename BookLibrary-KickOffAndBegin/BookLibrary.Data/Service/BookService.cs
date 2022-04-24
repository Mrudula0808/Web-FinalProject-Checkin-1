using BookLibrary.Data.Common;
using BookLibrary.Data.DbContexts;
using BookLibrary.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace BookLibrary.Data.Service
{
    public class BookService
    {
        private readonly AppDbContext _db;
        public BookService(AppDbContext db)
        {
            _db = db;
            InitCategory();
        }

        public void InitCategory()
        {
            if (_db.Categories.Any())
            {
                return;
            }

            var categories = new List<string>{ "History", "Economic", "Poem", "Comedy", "Comic", "Science", "Health","Education", "Sport" } ;

            foreach (var name in categories)
            {
                var category = new Category
                {
                    CreatedDate = DateTime.Now,
                    Name = name,
                    Description = name,
                };
                _db.Add(category);
            }
            _db.SaveChanges();
        }

        public Result<List<Book>> GetBooks(string? title = default, string? author = default, string? publishedBy = default, int? publishedYear = default, Guid? categoryId = default)
        {
            title = title?.ToLower().Trim();
            List<Guid> bookIds = new();
            if (categoryId.HasValue)
            {
                var category = _db.Categories.Include(c => c.Books)
                    .FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    bookIds = category.Books.Select(c => c.Id).ToList();
                }
            }

            var books = _db.Books
                .WhereIf(!string.IsNullOrEmpty(title), b => b.Title.ToLower().Contains(title))
                .WhereIf(!string.IsNullOrEmpty(author), b => b.Author.ToLower().Contains(author))
                .WhereIf(!string.IsNullOrEmpty(publishedBy), b => b.PublishedBy.ToLower().Contains(publishedBy))
                .WhereIf(publishedYear.HasValue, b => b.PublishedYear == publishedYear)
                .WhereIf(categoryId.HasValue,b =>  bookIds.Contains(b.Id))
                .Include(book => book.Categories)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();

            return new(books);
        }

        public List<Category> GetCategories()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }

    }
}
