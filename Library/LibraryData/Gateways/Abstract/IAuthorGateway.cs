using LibraryData.Models;

namespace LibraryData.Gateways.Abstract;

public interface IAuthorGateway : IGateway<Author>
{
    Author DeleteAuthor(int id);
}