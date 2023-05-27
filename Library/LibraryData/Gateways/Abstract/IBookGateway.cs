namespace LibraryData.Gateways.Abstract;

using Models;

public interface IBookGateway : IGateway<Book>
{
    Book? GetByISBN(int ISBN);
    Book DeleteByISBN(int ISBN);
}