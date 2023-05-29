using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models;

public class Book : Entity
{
    public string ISBN { get; init; } = null!;

    public string Title { get; init; } = null!;

    public int AuthorId { get; init; }

    public DateTime? PublicationDate { get; init; } = null;


    public Author? Author { get; set; } = null;


    public Book(int? id, string isbn, string title, int authorId, DateTime? publicationDate)
    : base(id)
    {
        ISBN = isbn;
        Title = title;
        AuthorId = authorId;
        PublicationDate = publicationDate;
    }

    public Book()
    : base()
    { }
}