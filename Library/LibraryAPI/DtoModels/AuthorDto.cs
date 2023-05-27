namespace LibraryAPI.DtoModels;

public record AuthorDto(string GivenName = "", string FamilyName = "", DateTime? BirthDate = default);