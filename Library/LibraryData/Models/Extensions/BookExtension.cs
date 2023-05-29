namespace LibraryData.Models.Extensions;

using System.Text.RegularExpressions;
using Models;

public static class BookExtension
{
    public static bool IsValid(this Book book)
    {
        var rISBN = @"^ISBN13-(?:97[89])?-\d{1,5}-\d{1,7}-\d{1,6}-\d{1}$";
        var rTitle = @"^[A-Za-z0-9\s.,'""\-:()?!]+$";
        return Regex.Match(book.ISBN, rISBN).Success && Regex.Match(book.Title, rTitle).Success;
    }
}

/*
 * USED:
 * ISBN13-978-0-306-40615-7, ISBN13-978-1-84356-028-4, ISBN13-978-0-9767736-6-0,
 * ISBN13-978-0-13-235088-4, ISBN13-978-0-201-53082-1, ISBN13-978-0-7356-4877-1,
 * ISBN13-978-0-596-52068-7, ISBN13-978-0-07-352332-3, ISBN13-978-0-321-97205-9,
 * ISBN13-978-0-13-277289-0, ISBN13-978-0-671-02735-3, ISBN13-978-0-345-35388-9,
 * ISBN13-978-0-14-044924-2, ISBN13-978-0-8044-2957-6, ISBN13-978-0-06-112241-5,
 * ISBN13-978-0-451-17174-7, ISBN13-978-0-385-50420-8, ISBN13-978-0-399-53536-5,
 * ISBN13-978-0-553-38108-2, ISBN13-978-0-312-14494-8, ISBN13-978-0-7499-8133-4,
 * ISBN13-978-0-451-46800-9, ISBN13-978-0-571-22063-9, ISBN13-978-0-316-27277-4,
 * ISBN13-978-0-525-95414-9, ISBN13-978-0-14-311001-4, ISBN13-978-0-373-20092-5, 
 * ISBN13-978-0-517-88431-7
 *
 * UNUSED:
 * ISBN13-978-0-425-19949-3, ISBN13-978-0-316-31625-4
 * 
 * ^ISBN13-(?:97[89])?-\d{1,5}-\d{1,7}-\d{1,6}-\d{1}$
 * ^ISBN13-\d{3}-\d-\d{6}-\d{1}$|^ISBN13-\d{3}-\d{5}-\d{3}-\d{1}$
 */