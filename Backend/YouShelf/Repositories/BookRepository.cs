using Dapper;
using System.Data;
using YouShelf.Dtos;
using YouShelf.Models;

namespace YouShelf.Repositories;

public class BookRepository
{
    private readonly IDbConnection _db;
    public BookRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Book>> GetBookList(int userId, int limit, int offset)
    {
        string sql = @"SELECT * FROM BOOKS WHERE UserId = @UserId LIMIT @Limit OFFSET @Offset";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId, Limit = limit, Offset = offset });

        return books;
    }
    public async Task<IEnumerable<Book>> SearchBookTitle(int userId, string title, int limit, int offset)
    {
        string sql = @"SELECT * FROM BOOKS WHERE UserId = @UserId AND Title LIKE @Title LIMIT @Limit OFFSET @Offset";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId, Title = title, Limit = limit, Offset = offset });

        return books;
    }
    public async Task<IEnumerable<Book>> SearchBookAuthor(int userId, string author, int limit, int offset)
    {
        string sql = @"SELECT * FROM BOOKS WHERE UserId = @UserId AND Author LIKE @Author LIMIT @Limit OFFSET @Offset";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId, Author = author, Limit = limit, Offset = offset });

        return books;
    }

    public async Task<Book> AddBook(BookDto dto, int userId)
    {
        Console.WriteLine("adding book");
        string sql = @"INSERT INTO Books (Title, Author, Description, ReleaseDate, ImageUrl, Review, UserId) VALUES (@Title, @Author, @Description, @ReleaseDate, @ImageUrl, @Review, @UserId); SELECT * from Books where BookId = last_insert_rowid()";
        Book book = await _db.QuerySingleAsync<Book>(sql, new
        {
            Title = dto.Title,
            Author = dto.Author,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate.ToString("yyyy-MM-dd"),
            ImageUrl = dto.ImageUrl,
            Review = dto.Review,
            UserId = userId
        });
        return book;
    }
}
