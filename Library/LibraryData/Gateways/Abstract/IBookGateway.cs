namespace LibraryData.Gateways.Abstract;

using Models;

public interface IBookGateway : IGateway<Book>
{
    Book? GetByISBN(string ISBN);
    Book DeleteByISBN(string ISBN);
}