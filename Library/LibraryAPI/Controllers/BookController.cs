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

    // GET
    [HttpGet, ActionName(nameof(GetAll))]
    public IActionResult GetAll()
    {
        try
        {
            var books = _bookGateway.GetAll();

            if (books is null)
            {
                return BadRequest("Books Not Found");
            }

            var bookDtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationDate, b.ISBN));
            return Ok(bookDtos);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpGet("{size:int}/{page:int}"), ActionName(nameof(GetByPage))]
    public IActionResult GetByPage([FromRoute] int size, [FromRoute] int page)
    {
        try
        {
            var books = _bookGateway.GetByPage(size, page);

            if (books is null)
            {
                return BadRequest("Books Not Found");
            }

            var bookDtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationDate, b.ISBN));
            return Ok(bookDtos);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpGet("{id:int}"), ActionName(nameof(GetById))]
    public IActionResult GetById([FromRoute] int id)
    {
        try
        {
            var book = _bookGateway.GetById(id);

            if (book is null)
            {
                return BadRequest("Book Not Found");
            }

            var bookDto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationDate, book.ISBN);
            return Ok(bookDto);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpGet("{isbn}"), ActionName(nameof(GetByISBN))]
    public IActionResult GetByISBN([FromRoute] string isbn)
    {
        try
        {
            var book = _bookGateway.GetByISBN(isbn);

            if (book is null)
            {
                return BadRequest("Book Not Found");
            }

            var bookDto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationDate, book.ISBN);
            return Ok(bookDto);
        }
        catch
        {
            return Problem();
        }
    }

    // POST
    [HttpPost, ActionName(nameof(PostOne))]
    public IActionResult PostOne([FromBody] BookDto dto)
    {
        try
        {
            Book book;

            var count = 0;
            //
            book = new Book(null, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate);
            var bookIn = _bookGateway.Insert(book);
            count++;

            return Ok(count);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpPost, ActionName(nameof(PostMulti))]
    public IActionResult PostMulti([FromBody] IList<BookDto> dtos)
    {
        try
        {
            var books = new List<Book>();
            foreach (var dto in dtos)
            {
                books.Add(new Book(null, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate));
            }
            _bookGateway.InsertMulti(books);
            return Ok(books.Count);
        }
        catch
        {
            return Problem();
        }
    }

    // PUT
    [HttpPut("{id:int}"), ActionName(nameof(PutById))]
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
            book = new Book(bookOld.Id, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate);
            _bookGateway.Update(book);

            return Ok(id);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpPut("{isbn}"), ActionName(nameof(PutByISBN))]
    public IActionResult PutByISBN([FromRoute] string isbn, [FromBody] StandardBookDto dto)
    {
        try
        {
            var bookOld = _bookGateway.GetByISBN(isbn.ToString());
            if (bookOld is null)
            {
                return BadRequest("Book Not Found");
            }

            Book book;
            book = new Book(bookOld.Id, isbn, dto.Title, dto.AuthorId, dto.PublicationDate);
            _bookGateway.Update(book);

            return Ok(isbn);
        }
        catch
        {
            return Problem();
        }
    }

    // DELETE
    [HttpDelete("{id:int}"), ActionName(nameof(DeleteById))]
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

    [HttpDelete("{isbn}"), ActionName(nameof(DeleteByISBN))]
    public IActionResult DeleteByISBN([FromRoute] string isbn)
    {
        try
        {
            if (_bookGateway.GetByISBN(isbn) is null)
            {
                return BadRequest("Book Not Found");
            }
            else
            {
                _bookGateway.DeleteByISBN(isbn.ToString());
                return StatusCode(200);
            }
        }
        catch
        {
            return Problem();
        }
    }
}