using BookLibrary.Data.Common;
using BookLibrary.Data.Entity;
using BookLibrary.Data.Service;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Index(BooksVm vm)
        {
            var result = _bookService.GetBooks(vm.Title, vm.Author, vm.PublishedBy, vm.PublishedYear, vm.CategoryId);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }
            var categories = _bookService.GetCategories();
            vm.Categories = categories;
            vm.Books = result.Data;
            return View(vm);
        }
    }
}
