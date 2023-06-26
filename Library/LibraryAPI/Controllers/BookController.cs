namespace LibraryAPI.Controllers;

using DtoModels;
using LibraryData.Models;
using LibraryData.Gateways.Abstract;
using Microsoft.AspNetCore.Mvc;

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
            if (books is null) return BadRequest("Books Not Found");
            var dtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationDate, b.ISBN));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{size:int}/{page:int}"), ActionName(nameof(GetByPage))]
    public IActionResult GetByPage([FromRoute] int size, [FromRoute] int page)
    {
        try
        {
            var books = _bookGateway.GetByPage(size, page);
            if (books is null) return BadRequest("Books Not Found");
            var dtos = books.Select(b => new AuthenticBookDto(b.Title, $"{b.Author!.GivenName} {b.Author!.FamilyName}", b.PublicationDate, b.ISBN));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id:int}"), ActionName(nameof(GetById))]
    public IActionResult GetById([FromRoute] int id)
    {
        try
        {
            var book = _bookGateway.GetById(id);
            if (book is null) return BadRequest("Book Not Found");
            var dto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationDate, book.ISBN);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{isbn}"), ActionName(nameof(GetByISBN))]
    public IActionResult GetByISBN([FromRoute] string isbn)
    {
        try
        {
            var book = _bookGateway.GetByISBN(isbn);
            if (book is null) return BadRequest("Book Not Found");
            var dto = new AuthenticBookDto(book.Title, $"{book.Author!.GivenName} {book.Author!.FamilyName}", book.PublicationDate, book.ISBN);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // POST
    [HttpPost, ActionName(nameof(PostOne))]
    public IActionResult PostOne([FromBody] BookDto dto)
    {
        try
        {
            Book book;
            book = new Book(null, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate);
            _bookGateway.Insert(book);
            return Ok(1);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost, ActionName(nameof(PostMulti))]
    public IActionResult PostMulti([FromBody] IList<BookDto> dtos)
    {
        try
        {
            var books = new List<Book>();
            foreach (var dto in dtos) books.Add(new Book(null, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate));
            _bookGateway.InsertMulti(books);
            return Ok(books.Count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // PUT
    [HttpPut("{id:int}"), ActionName(nameof(PutById))]
    public IActionResult PutById([FromRoute] int id, [FromBody] BookDto dto)
    {
        try
        {
            var bookOld = _bookGateway.GetById(id);
            if (bookOld is null) return BadRequest("Book Not Found");
            Book book;
            book = new Book(bookOld.Id, dto.ISBN, dto.Title, dto.AuthorId, dto.PublicationDate);
            _bookGateway.Update(book);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{isbn}"), ActionName(nameof(PutByISBN))]
    public IActionResult PutByISBN([FromRoute] string isbn, [FromBody] StandardBookDto dto)
    {
        try
        {
            var bookOld = _bookGateway.GetByISBN(isbn);
            if (bookOld is null) return BadRequest("Book Not Found");
            Book book;
            book = new Book(bookOld.Id, isbn, dto.Title, dto.AuthorId, dto.PublicationDate);
            _bookGateway.Update(book);
            return Ok(isbn);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // DELETE
    [HttpDelete("{id:int}"), ActionName(nameof(DeleteById))]
    public IActionResult DeleteById([FromRoute] int id)
    {
        try
        {
            var bookOld = _bookGateway.GetById(id);
            if (bookOld is null) return BadRequest("Book Not Found");
            _bookGateway.Delete(id);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{isbn}"), ActionName(nameof(DeleteByISBN))]
    public IActionResult DeleteByISBN([FromRoute] string isbn)
    {
        try
        {
            var bookOld = _bookGateway.GetByISBN(isbn);
            if (bookOld is null) return BadRequest("Book Not Found");
            _bookGateway.DeleteByISBN(isbn);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}