namespace LibraryData.Gateways;

using Microsoft.EntityFrameworkCore;
using Abstract;
using Data;
using Models;
using System.Linq;
using LibraryData.Models.Extensions;

public class BookGateway : IBookGateway
{
    private readonly LibraryContext _context;

    public BookGateway(LibraryContext context) 
    {
	    _context = context; 
    }

    // GET
    public IEnumerable<Book> GetAll()
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author);
        return book;
    }

    public IEnumerable<Book>? GetByPage(int size, int page)
    {
        var books = _context.Books.AsNoTracking().Include(b => b.Author);
        if (size <= 0) return null;
        if ((page - 1) < 0) return null;
        if ((page - 1) > 0 && (page - 1) * size >= books.Count()) return null;
        var output = (page - 1) == 0 ? books : books.Skip(size * (page - 1));
        var pagesize = output.Count();
        return output.Take(pagesize < size ? pagesize : size);
    }

    public Book? GetById(int id) 
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author).SingleOrDefault(b => b.Id == id);
        return book;
    }

    public Book? GetByISBN(string isbn)
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author).FirstOrDefault(b => b.ISBN == isbn);
        return book;
    }

    // INSERT
    public Book Insert(Book entity)
    {
        if (entity is null) throw new Exception("No Entity");
        if (entity.Id is not null) throw new Exception("Not Null Id");
        if (!entity.IsValid()) throw new Exception("Not Book");
        if (_context.Books.Any(b => b.ISBN == entity.ISBN)) throw new Exception("Existing Entity");
        if (!_context.Authors.Any(a => a.Id == entity.AuthorId)) throw new Exception("Not Author Id");
        var book = _context.Books.Add(entity);
        _context.SaveChanges();
        return book.Entity;
    }

    public IEnumerable<Book>? InsertMulti(IEnumerable<Book>? entities)
    {
        if (entities is null) throw new Exception("No Entities");
        if (entities.Any(e => e.Id != null)) throw new Exception("Not Null Ids");
        if (entities.Any(e => !e.IsValid())) throw new Exception("Not Authors");
        foreach (var entity in entities) if (_context.Books.Any(b => b.ISBN == entity.ISBN)) throw new Exception("Existing Entities");
        foreach (var entity in entities) if (!_context.Authors.Any(a => a.Id == entity.AuthorId)) throw new Exception("Not Authors Id");
        var books = new List<Book>();
        foreach (var entity in entities) books.Add(_context.Books.Add(entity).Entity);
        _context.SaveChanges();
        return books;
    }

    // UPDATE
    public Book Update(Book entity)
    {
        if (entity.Id is null) throw new Exception("Null Id");
        if (!entity.IsValid()) throw new Exception("Not Book");
        if (!_context.Authors.Any(a => a.Id == entity.AuthorId)) throw new Exception("Not Author Id");
        var intId = (int) entity.Id;
        var entityOld = GetById(intId);
        if (entityOld is null) throw new Exception("Null Entity");
        if (entity.ISBN != entityOld.ISBN && _context.Books.Any(b => b.ISBN == entity.ISBN)) throw new Exception("Existing Entities");
        var entityNew = new Book(intId, entity.ISBN, entity.Title, entity.AuthorId, entity.PublicationDate);
        var book = _context.Books.Update(entityNew);
        _context.SaveChanges();
        book.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entity.AuthorId);
        return book.Entity;
    }

    // DELETE
    public Book Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("Null Entity");
        var book = _context.Books.Remove(entityOld);
        _context.SaveChanges();
        book.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entityOld.AuthorId);
        return book.Entity;
    }

    public Book DeleteByISBN(string isbn)
    {
        var entityOld = GetByISBN(isbn);
        if (entityOld is null) throw new Exception("Null Entity");
        var book = _context.Books.Remove(entityOld);
        _context.SaveChanges();
        book.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entityOld.AuthorId);
        return book.Entity;
    }
}