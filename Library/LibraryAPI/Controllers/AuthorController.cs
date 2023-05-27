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


    [HttpGet, ActionName("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
            var authors = _authorGateway.GetAll();

            if (authors is null)
            {
                return BadRequest("Authors Not Found");
            }

            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet, ActionName("GetAllAuthors")]
    public IActionResult GetAllAuthors()
    {
        try
        {
            var authors = _authorGateway.GetAllEntities();

            if (authors is null)
            {
                return BadRequest("Authors Not Found");
            }

            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpGet("{size:int}/{page:int}"), ActionName("GetAuthorsByPage")]
    public IActionResult GetAuthorsByPage([FromRoute] int size, [FromRoute] int page)
    {
        try
        {
            var authors = _authorGateway.GetEntitiesByPage(size, page);

            if (authors is null)
            {
                return BadRequest("Authors Not Found");
            }

            var authorsDtos = authors.Select(a => new AuthorDto(a.GivenName, a.FamilyName, a.BirthDate));
            return Ok(authorsDtos);
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
            var author = _authorGateway.GetById(id);

            if (author is null)
            {
                return BadRequest("Author Not Found");
            }

            var authorDto = new AuthorDto(author.GivenName, author.FamilyName, author.BirthDate);
            return Ok(authorDto);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPost, ActionName("PostOne")]
    public IActionResult Post([FromBody] AuthorDto authorDto)
    {
        try
        {
            Author author;

            var count = 0;
            //
            author = new Author(null, authorDto.GivenName, authorDto.FamilyName, authorDto.BirthDate);
            var authorIn = _authorGateway.Insert(author);
            count++;

            return Ok(count);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPost, ActionName("PostListWrong")]
    public IActionResult PostWrong([FromBody] IList<AuthorDto> authorDtos)
    {
        try
        {
            Author author;

            var count = 0;
            foreach (var authorDto in authorDtos)
            {
                author = new Author(null, authorDto.GivenName, authorDto.FamilyName, authorDto.BirthDate);
                var authorIn = _authorGateway.Insert(author);
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
    public IActionResult Post([FromBody] IList<AuthorDto> authorDtos)
    {
        try
        {
            var authors = new List<Author>();
            foreach (var authorDto in authorDtos)
            {
                authors.Add(new Author(null, authorDto.GivenName, authorDto.FamilyName, authorDto.BirthDate));
            }
            _authorGateway.InsertMulti(authors);
            return Ok(authors.Count);
        }
        catch
        {
            return Problem();
        }
    }


    [HttpDelete("{id:int}"), ActionName("DeleteById")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        try
        {
            if (_authorGateway.GetById(id) is null)
            {
                return BadRequest("Authur Not Found");
            }
            else
            {
                _authorGateway.Delete(id);
                return StatusCode(200);
            }
        }
        catch
        {
            return Problem();
        }
    }


    [HttpDelete("{id:int}"), ActionName("DeleteAuthorById")]
    public IActionResult DeleteAuthorById([FromRoute] int id)
    {
        try
        {
            if (_authorGateway.GetById(id) is null)
            {
                return BadRequest("Authur Not Found");
            }
            else
            {
                _authorGateway.DeleteAuthor(id);
                return StatusCode(200);
            }
        }
        catch
        {
            return Problem();
        }
    }


    [HttpPut("{id:int}"), ActionName("PutById")]
    public IActionResult PutById([FromRoute] int id, [FromBody] AuthorDto authorDto)
    {
        try
        {
            var authorOld = _authorGateway.GetById(id);
            if (authorOld is null)
            {
                return BadRequest("Author Not Found");
            }

            Author author;
            author = new Author(authorOld.Id, authorDto.GivenName, authorDto.FamilyName, authorDto.BirthDate);
            _authorGateway.Update(author);

            return Ok(id);
        }
        catch
        {
            return Problem();
        }
    }
}