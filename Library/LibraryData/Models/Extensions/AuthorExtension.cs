namespace LibraryData.Models.Extensions;

using System.Text.RegularExpressions;
using Models;

public static class AuthorExtension
{
    public static bool IsValid(this Author author)
    {
        var rName = @"^[A-Z][A-Za-z]*(?: +[A-Z][A-Za-z]*)*$";
        return Regex.Match(author.FamilyName, rName).Success && Regex.Match(author.FamilyName, rName).Success;
    }
}