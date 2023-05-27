namespace LibraryAPI.DtoModels;

public record BaseBookDto(string Title = "", DateTime? PublicationYear = default);

public record StandardBookDto(string Title = "", int AuthorId = default, DateTime? PublicationYear = default) 
    : BaseBookDto(Title, PublicationYear);

public record OriginalBookDto(string Title = "", string AuthorFullName = "", DateTime? PublicationYear = default) 
    : BaseBookDto(Title, PublicationYear);

public record BookDto(string Title = "", int AuthorId = default, DateTime? PublicationYear = default, int ISBN = default) 
    : StandardBookDto(Title, AuthorId, PublicationYear);

public record AuthenticBookDto(string Title = "", string AuthorFullName = "", DateTime? PublicationYear = default, int ISBN = default) 
    : OriginalBookDto(Title, AuthorFullName, PublicationYear);
