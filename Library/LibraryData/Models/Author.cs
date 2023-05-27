namespace LibraryData.Models;

public record Author(int? Id = default, string GivenName = "", string FamilyName = "", DateTime? BirthDate = default) : Entity(Id)
{
    public IEnumerable<Book> Messages = new HashSet<Book>();
}