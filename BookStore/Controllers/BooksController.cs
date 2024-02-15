using BookStore.CustomExceptions;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>
    {
        new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Fiction" },
        new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction" },
        new Book { Id = 3, Title = "To eat a Mockingbird", Author = "Leelo", Genre = "horror" },
        new Book { Id = 4, Title = "To sheet a Mockingbird", Author = "Leela", Genre = "comedy" },
        new Book { Id = 5, Title = "To meal a Mockingbird", Author = "Lenda", Genre = "thriller" },
        new Book { Id = 6, Title = "To food a Mockingbird", Author = "Leo", Genre = "detective" },
        new Book { Id = 7, Title = "To out a Mockingbird", Author = "Leeshta", Genre = "love" },
        // ... other books
    };

        [HttpGet("search")]
        public ActionResult<IEnumerable<Book>> SearchBooks(string? title = null, string? genre = null, string? author = null)
        {
            var results = books
                .Where(book =>
                    (string.IsNullOrWhiteSpace(title) || book.Title.ToLower().Contains(title.ToLower())) &&
                    (string.IsNullOrWhiteSpace(genre) || book.Genre?.ToLower().Contains(genre.ToLower()) == true) &&
                    (string.IsNullOrWhiteSpace(author) || book.Author?.ToLower().Contains(author.ToLower()) == true))
                .ToList();

            if (results.Count == 0)
            {
                throw new NotFoundException("No books found matching the provided criteria.");
            }

            return Ok(results);
        }
    }
}
