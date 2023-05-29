using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models;

public class Author : Entity
{
    public string GivenName { get; init; } = null!;

    public string FamilyName { get; init; } = null!;

    public DateTime? BirthDate { get; init; } = null;


    public IEnumerable<Book> Books = new HashSet<Book>();


    public Author(int? id, string givenName, string familyName, DateTime? birthDate) 
    : base(id)
    {
        GivenName = givenName;
        FamilyName = familyName;
        BirthDate = birthDate;
    }

    public Author()
    : base()
    { }
}