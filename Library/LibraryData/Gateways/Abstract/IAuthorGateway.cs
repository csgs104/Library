using LibraryData.Models;

namespace LibraryData.Gateways.Abstract;

public interface IAuthorGateway : IGateway<Author>
{
    public IEnumerable<Author>? GetAllAuthors();
    public Author? GetAuthorById(int id);
    public Author DeleteAuthor(int id);
}