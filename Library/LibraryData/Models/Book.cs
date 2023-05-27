namespace LibraryData.Models;

public record Book(int? Id = default, string Title = "", int AuthorId = default, DateTime? PublicationYear = default, int ISBN = default) : Entity(Id)
{
    public Author? Author { get; set; }
}