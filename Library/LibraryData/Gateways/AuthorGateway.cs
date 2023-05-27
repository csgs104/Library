namespace LibraryData.Gateways;

using Microsoft.EntityFrameworkCore;
using Abstract;
using Data;
using Models;


public class AuthorGateway : IAuthorGateway
{
    private readonly LibraryContext _context;

    public AuthorGateway(LibraryContext context) => _context = context;


    public IEnumerable<Author> GetAll()
    {
        var authors = _context.Authors.AsNoTracking();
        return authors;
    }


    public IEnumerable<Author>? GetAllEntities()
    {
        var books = _context.Books.AsNoTracking().Include(b => b.Author);
        if (books is null) return null;
        var authors = books.Where(b => b.Author != null).Select(b => b.Author);
        if (authors is null) return null;
        return (IQueryable<Author>?) authors.Distinct();
    }


    public IEnumerable<Author>? GetEntitiesByPage(int size, int page)
    {
        var authors = _context.Authors.AsNoTracking();
        return authors.Skip(size * page).Take(size);
    }


    public IEnumerable<Author>? GetByIds(IEnumerable<int> ids)
    {
        var authors = _context.Authors.AsNoTracking();
        int found;
        foreach (var author in authors)
        {
            found = 0;
            foreach (var id in ids)
            {
                if (author.Id == id)
                {
                    found++;
                    yield return author;
                }
                else continue;
            }
            if (found != 1) throw new Exception("Null Entity");
            found = 0;
        }
    }


    public Author? GetById(int id)
    {
        var author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == id);
        return author;
    }


    public Author Insert(Author entity)
    {
        if (entity.Id is not null) throw new Exception("Not Null Id");

        var author = _context.Authors.Add(entity);
        _context.SaveChanges();

        return author.Entity;
    }


    public IEnumerable<Author>? InsertMulti(IEnumerable<Author>? entities)
    {
        if (entities.Any(e => e.Id != null)) throw new Exception("Not Null Id");

        var books = new List<Author >();
        foreach (var entity in entities)
        {
            books.Add(_context.Authors.Add(entity).Entity);
        }
        _context.SaveChanges();

        return books;
    }


    public Author Update(Author entity)
    {
        if (entity.Id is null) throw new Exception("Null Id");
        var intId = (int) entity.Id;
        var entityOld = GetById(intId);
        if (entityOld is null) throw new Exception("Null Entity");

        var author = _context.Authors.Update(entityOld with { Id = intId, GivenName = entity.GivenName, FamilyName = entity.FamilyName });
        _context.SaveChanges();

        return author.Entity;
    }


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
        var authors = GetAllEntities();

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