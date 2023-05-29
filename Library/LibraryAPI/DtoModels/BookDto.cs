namespace LibraryAPI.DtoModels;

public record BaseBookDto(string Title = "", DateTime? PublicationDate = default);

public record StandardBookDto(string Title = "", int AuthorId = default, DateTime? PublicationDate = default) 
    : BaseBookDto(Title, PublicationDate);

public record OriginalBookDto(string Title = "", string AuthorFullName = "", DateTime? PublicationDate = default) 
    : BaseBookDto(Title, PublicationDate);

public record BookDto(string Title = "", int AuthorId = default, DateTime? PublicationDate = default, string ISBN = "") 
    : StandardBookDto(Title, AuthorId, PublicationDate);

public record AuthenticBookDto(string Title = "", string AuthorFullName = "", DateTime? PublicationDate = default, string ISBN = "") 
    : OriginalBookDto(Title, AuthorFullName, PublicationDate);
