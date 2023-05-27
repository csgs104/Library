namespace LibraryAPI.Controllers;

using System.Linq;
using DtoModels;
using LibraryData.Models;
using LibraryData.Gateways.Abstract;
using Microsoft.AspNetCore.Mvc;
using LibraryData.Gateways;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookController : ControllerBase
{
    private readonly IBookGateway _bookGateway;
    private readonly ILogger<BookController> _logger;

    public BookController(IBookGateway bookGateway, ILogger<BookController> logger)
    {
        _bookGateway = bookGateway;
        _logger = logger;
    }


    [HttpGet, ActionName("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
            var books = _bookGateway.GetAll();

            if (books is null)
            {
                return BadRequest("Books Not Found");
            }

            var bookDtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationYear, b.ISBN));
            return Ok(bookDtos);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet, ActionName("GetAllBooks")]
    public IActionResult GetAllBooks()
    {
        try
        {
            var books = _bookGateway.GetAllEntities();

            if (books is null)
            {
                return BadRequest("Books Not Found");
            }

            var bookDtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationYear, b.ISBN));
            return Ok(bookDtos);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet("{size:int}/{page:int}"), ActionName("GetBooksByPage")]
    public IActionResult GetAllBooksByPage([FromRoute] int size, [FromRoute] int page)
    {
        try
        {
            var books = _bookGateway.GetEntitiesByPage(size, page);

            if (books is null)
            {
                return BadRequest("Book Not Found");
            }

            var bookDtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationYear, b.ISBN));
            return Ok(bookDtos);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet("{id:int}"), ActionName("GetById")]
    public IActionResult GetById([FromRoute] int id)
    {
        try
        {
            var book = _bookGateway.GetById(id);

            if (book is null)
            {
                return BadRequest("Book Not Found");
            }

            var bookDto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationYear, book.ISBN);
            return Ok(bookDto);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet("{ISBN:int}"), ActionName("GetByISBN")]
    public IActionResult GetByISBN([FromRoute] int ISBN)
    {
        try
        {
            var book = _bookGateway.GetByISBN(ISBN);

            if (book is null)
            {
                return BadRequest("Book Not Found");
            }

            var bookDto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationYear, book.ISBN);
            return Ok(bookDto);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPost, ActionName("PostOne")]
    public IActionResult Post([FromBody] BookDto bookDto)
    {
        try
        {
            Book book;

            var count = 0;
            //
            book = new Book(null, bookDto.Title, bookDto.AuthorId, bookDto.PublicationYear, bookDto.ISBN);
            var bookIn = _bookGateway.Insert(book);
            count++;

            return Ok(count);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPost, ActionName("PostListWrong")]
    public IActionResult PostWrong([FromBody] IList<BookDto> bookDtos)
    {
        try
        {
            Book book;

            var count = 0;
            foreach (var bookDto in bookDtos)
            {
                book = new Book(null, bookDto.Title, bookDto.AuthorId, bookDto.PublicationYear, bookDto.ISBN);
                _bookGateway.Insert(book);
                count++;
	        }

            return Ok(count);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPost, ActionName("PostList")]
    public IActionResult Post([FromBody] IList<BookDto> bookDtos)
    {
        try
        {
            var books = new List<Book>();
            foreach (var bookDto in bookDtos)
            {
                books.Add(new Book(null, bookDto.Title, bookDto.AuthorId, bookDto.PublicationYear, bookDto.ISBN));
            }
            _bookGateway.InsertMulti(books);
            return Ok(books.Count);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpDelete("{ISBN:int}"), ActionName("DeleteISBN")]
    public IActionResult DeleteByISBN([FromRoute] int ISBN)
    {
        try
        {
            if (_bookGateway.GetByISBN(ISBN) is null)
            {
                return BadRequest("Book Not Found");
            }
            else
            {
                _bookGateway.DeleteByISBN(ISBN);
                return StatusCode(200);
            }
        }
        catch
        {
            return Problem();
        }
    }


    [HttpDelete("{id:int}"), ActionName("DeleteId")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        try
        {
            if (_bookGateway.GetById(id) is null)
            {
                return BadRequest("Book Not Found");
            }
            else
            {
                _bookGateway.Delete(id);
                return StatusCode(200);
            }
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPut("{id:int}"), ActionName("PutById")]
    public IActionResult PutById([FromRoute] int id, [FromBody] BookDto dto)
    {
        try
        {
            var bookOld = _bookGateway.GetById(id);
            if (bookOld is null)
            {
                return BadRequest("Book Not Found");
            }

            Book book;
            book = new Book(bookOld.Id, dto.Title, dto.AuthorId, dto.PublicationYear, dto.ISBN);
            _bookGateway.Update(book);

            return Ok(id);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPut("{ISBN:int}"), ActionName("PutByISBN")]
    public IActionResult PutByISBN([FromRoute] int ISBN, [FromBody] StandardBookDto dto)
    {
        try 
	    {
            var bookOld = _bookGateway.GetByISBN(ISBN);
            if (bookOld is null)
            {
                return BadRequest("Book Not Found");
            }

            Book book;
            book = new Book(bookOld.Id, dto.Title, dto.AuthorId, dto.PublicationYear, ISBN);
            _bookGateway.Update(book);

            return Ok(ISBN);
        }
        catch
        {
            return Problem();
        }
    }
}