namespace LibraryData.Gateways;

using Microsoft.EntityFrameworkCore;
using Abstract;
using Data;
using Models;
using Models.Extensions;

public class AuthorGateway : IAuthorGateway
{
    private readonly LibraryContext _context;

    public AuthorGateway(LibraryContext context) 
    {
	    _context = context; 
    }

    // GET
    public IEnumerable<Author> GetAll()
    {
        var authors = _context.Authors.AsNoTracking();
        return authors;
    }

    public IEnumerable<Author>? GetAllAuthors()
    {
        var books = _context.Books.AsNoTracking().Include(b => b.Author);
        if (books is null) return null;
        var authors = books.Where(b => b.Author != null).Select(b => b.Author);
        if (authors is null) return null;
        return (IQueryable<Author>?) authors.Distinct();
    }

    public IEnumerable<Author>? GetByPage(int size, int page)
    {
        var authors = _context.Authors.AsNoTracking();
        return authors.Skip(size * page).Take(size);
    }

    public Author? GetById(int id)
    {
        var author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == id);
        return author;
    }

    public Author? GetAuthorById(int id)
    {
        var authors = GetAllAuthors();
        if (authors is null) return null;
        var author = authors.SingleOrDefault(a => a.Id == id);
        return author;
    }

    // INSERT
    public Author Insert(Author entity)
    {
        if (entity.Id is not null) throw new Exception("Not Null Id");
        if (!entity.IsValid()) throw new Exception("Not Author");

        var author = _context.Authors.Add(entity);
        _context.SaveChanges();

        return author.Entity;
    }

    public IEnumerable<Author>? InsertMulti(IEnumerable<Author>? entities)
    {
        if (entities is null) throw new Exception("No Entities");
        if (entities.Any(e => e.Id != null)) throw new Exception("Not Null Ids");
        if (entities.Any(e => !e.IsValid())) throw new Exception("Not Authors");

        var books = new List<Author >();
        foreach (var entity in entities)
        {
            books.Add(_context.Authors.Add(entity).Entity);
        }
        _context.SaveChanges();

        return books;
    }

    // UPDATE
    public Author Update(Author entity)
    {
        if (entity.Id is null) throw new Exception("Null Id");
        if (!entity.IsValid()) throw new Exception("Not Author");

        var intId = (int) entity.Id;
        var entityOld = GetById(intId);
        if (entityOld is null) throw new Exception("Null Entity");

        var entityNew = new Author(intId, entity.GivenName, entity.FamilyName, entity.BirthDate);
        var author = _context.Authors.Update(entityNew);
        _context.SaveChanges();

        return author.Entity;
    }

    // DELETE
    public Author Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("Null Entity");

        var author = _context.Authors.Remove(entityOld);
        _context.SaveChanges();

        return author.Entity;
    }

    public Author DeleteAuthor(int id)
    {
        var authors = GetAllAuthors();

        Author? entityOld = null;
        if (authors is not null)
        {
            if (authors.Any(a => a.Id == id)) throw new Exception("Existing Author");
            else entityOld = GetById(id);
        }
        else
        { 
	        entityOld = GetById(id); 
	    }

        if (entityOld is null) throw new Exception("Null Entity");

        var author = _context.Authors.Remove(entityOld);
        _context.SaveChanges();

        return author.Entity;
    }
}