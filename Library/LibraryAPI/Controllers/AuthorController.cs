namespace LibraryAPI.Controllers;

using DtoModels;
using LibraryData.Models;
using LibraryData.Gateways.Abstract;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorGateway _authorGateway;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(IAuthorGateway authorGateway, ILogger<AuthorController> logger)
    {
        _authorGateway = authorGateway;
        _logger = logger;
    }

    // GET
    [HttpGet, ActionName(nameof(GetAll))]
    public IActionResult GetAll()
    {
        try
        {
            var authors = _authorGateway.GetAll();
            if (authors is null) return BadRequest("Authors Not Found");
            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet, ActionName(nameof(GetAllAuthors))]
    public IActionResult GetAllAuthors()
    {
        try
        {
            var authors = _authorGateway.GetAllAuthors();
            if (authors is null) return BadRequest("Authors Not Found");
            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
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
            var authors = _authorGateway.GetByPage(size, page);
            if (authors is null) return BadRequest("Authors Not Found");
            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
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
            var author = _authorGateway.GetById(id);
            if (author is null) return BadRequest("Author Not Found");
            var authorDto = new AuthorDto(author.GivenName, author.FamilyName, author.BirthDate);
            return Ok(authorDto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id:int}"), ActionName(nameof(GetAuthorById))]
    public IActionResult GetAuthorById([FromRoute] int id)
    {
        try
        {
            var author = _authorGateway.GetAuthorById(id);
            if (author is null) return BadRequest("Author Not Found");
            var authorDto = new AuthorDto(author.GivenName, author.FamilyName, author.BirthDate);
            return Ok(authorDto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // POST
    [HttpPost, ActionName(nameof(PostOne))]
    public IActionResult PostOne([FromBody] AuthorDto dto)
    {
        try
        {
            Author author;
            author = new Author(null, dto.GivenName, dto.FamilyName, dto.BirthDate);
            var authorIn = _authorGateway.Insert(author);
            return Ok(1);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost, ActionName(nameof(PostMulti))]
    public IActionResult PostMulti([FromBody] IList<AuthorDto> dtos)
    {
        try
        {
            var authors = new List<Author>();
            foreach (var dto in dtos) authors.Add(new Author(null, dto.GivenName, dto.FamilyName, dto.BirthDate));
            _authorGateway.InsertMulti(authors);
            return Ok(authors.Count);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // PUT
    [HttpPut("{id:int}"), ActionName(nameof(PutById))]
    public IActionResult PutById([FromRoute] int id, [FromBody] AuthorDto dto)
    {
        try
        {
            var authorOld = _authorGateway.GetById(id);
            if (authorOld is null) return BadRequest("Author Not Found");
            Author author;
            author = new Author(authorOld.Id, dto.GivenName, dto.FamilyName, dto.BirthDate);
            _authorGateway.Update(author);
            return Ok(id);
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
            var authorOld = _authorGateway.GetById(id);
            if (authorOld is null) return BadRequest("Authur Not Found");
            _authorGateway.Delete(id);
             return StatusCode(200);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{id:int}"), ActionName(nameof(DeleteAuthorById))]
    public IActionResult DeleteAuthorById([FromRoute] int id)
    {
        try
        {
            var authorOld = _authorGateway.GetById(id);
            if (authorOld is null) return BadRequest("Authur Not Found");
            _authorGateway.DeleteAuthor(id);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}