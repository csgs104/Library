﻿namespace LibraryData.Gateways;

using Microsoft.EntityFrameworkCore;
using Abstract;
using Data;
using Models;
using System.Linq;

public class BookGateway : IBookGateway
{
    private readonly LibraryContext _context;

    public BookGateway(LibraryContext context) => _context = context;


    public IEnumerable<Book> GetAll()
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author);
        return book;
    }


    public IEnumerable<Book> GetAllEntities()
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author);
        return book;
    }


    public IEnumerable<Book> GetEntitiesByPage(int size, int page)
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author);
        return book.Skip(size * page).Take(size);
    }


    public IEnumerable<Book>? GetByIds(IEnumerable<int> ids)
    {
        var books = _context.Books.AsNoTracking().Include(b => b.Author);
        int found;
        foreach (var book in books)
        {
            found = 0;
            foreach (var id in ids)
            {
                if (book.Id == id)
                {
                    found++;
                    yield return book;
                }
                else continue;
            }
            if (found != 1) throw new Exception("Null Entity");
            found = 0;
        }
    }


    public Book? GetById(int id)
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author).SingleOrDefault(b => b.Id == id);
        return book;
    }


    public Book? GetByISBN(int ISBN)
    {
        var book = _context.Books.AsNoTracking().Include(b => b.Author).FirstOrDefault(b => b.ISBN == ISBN);
        return book;
    }


    public Book Insert(Book entity)
    {
        if (entity.Id is not null) throw new Exception("Not Null Id");
        if (!_context.Authors.Any(a => a.Id == entity.AuthorId)) throw new Exception("Not Author Id");

        var book = _context.Books.Add(entity);
        _context.SaveChanges();

        return book.Entity;
    }


    public IEnumerable<Book>? InsertMulti(IEnumerable<Book>? entities)
    {
        if (entities.Any(e => e.Id != null)) throw new Exception("Not Null Id");

        foreach (var entity in entities)
        {
            if (!_context.Authors.Any(a => a.Id == entity.AuthorId)) throw new Exception("Not Author Id");
        }

        var books = new List<Book>();
        foreach (var entity in entities)
        {
            books.Add(_context.Books.Add(entity).Entity);
        }
        _context.SaveChanges();

        return books;
    }


    public Book Update(Book entity)
    {
        if (entity.Id is null) throw new Exception("Null Id");
        var intId = (int) entity.Id;
        var entityOld = GetById(intId);
        if (entityOld is null) throw new Exception("Null Entity");

        var message = _context.Books.Update(entityOld with { Id = intId, Title = entity.Title, AuthorId = entity.AuthorId });
        _context.SaveChanges();
        message.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entity.AuthorId);

        return message.Entity;
    }


    public Book Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("Null Entity");

        var message = _context.Books.Remove(entityOld);
        _context.SaveChanges();
        message.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entityOld.AuthorId);

        return message.Entity;
    }


    public Book DeleteByISBN(int ISBN)
    {
        var entityOld = GetByISBN(ISBN);
        if (entityOld is null) throw new Exception("Null Entity");

        var message = _context.Books.Remove(entityOld);
        _context.SaveChanges();
        message.Entity.Author = _context.Authors.AsNoTracking().SingleOrDefault(a => a.Id == entityOld.AuthorId);

        return message.Entity;
    }
}