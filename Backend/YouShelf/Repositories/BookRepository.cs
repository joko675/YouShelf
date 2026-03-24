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

    public async Task<IEnumerable<Book>> GetBookList(int userId)
    {
        string sql = @"SELECT * FROM BOOKS WHERE UserId = @UserId";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId });

        return books;
    }
    public async Task<IEnumerable<Book>> GetBookList(int userId, string title)
    {
        string sql = @"SELECT * FROM BOOKS WHERE (UserId = @UserId, Title = @Title)";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId, Title = title });

        return books;
    }

    public async Task<Book> AddBook(BookDto dto, int userId)
    {
        Console.WriteLine("adding book");
        string sql = @"INSERT INTO Books (Title, Description, ReleaseDate, ImageUrl, Review, UserId) VALUES (@Title, @Description, @ReleaseDate, @ImageUrl, @Review, @UserId); SELECT * from Books where BookId = last_insert_rowid()";

        Book book = await _db.QuerySingleAsync<Book>(sql, new
        {
            Title = dto.Title,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate.ToString("yyyy-MM-dd"),
            ImageUrl = dto.ImageUrl,
            Review = dto.Review,
            UserId = userId
        });
        return book;
    }
}
