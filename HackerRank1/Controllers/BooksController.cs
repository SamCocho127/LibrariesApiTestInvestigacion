using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryService.WebAPI.Data;
using LibraryService.WebAPI.Services;
using LibraryService.WebAPI.DTO;

namespace LibraryService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/libraries/{libraryId}/[Controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILibrariesService _librariesService;
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService, ILibrariesService librariesService)
        {
            _librariesService = librariesService;
            _booksService = booksService;
        }

        // Implement the functionalities below
        [HttpPost]
        public async Task<IActionResult> AddBook(int libraryId, [FromBody] BookForm bookForm)
        {
            Library currentLibrary = (await _librariesService.Get(new int[] { libraryId })).FirstOrDefault();
            if (currentLibrary == null)
                return NotFound();

            Book book = new Book()
            {
                Id = bookForm.Id,
                Category = bookForm.Category,
                Name = bookForm.Name,
                LibraryId = libraryId,
                Library = currentLibrary
            };

            return StatusCode(201, _booksService.Add(book));
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(int libraryId)
        {
            IEnumerable<Book> books =  await _booksService.Get(libraryId, new int[] { libraryId });
            
            return  books is null  || books.Count() == 0 ? NotFound(books) : Ok(books);
        }
    }
}