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
    public async Task<IEnumerable<Book>> SearchBookStatus(int userId, Status status, int limit, int offset)
    {
        string sql = @"SELECT * FROM BOOKS WHERE UserId = @UserId AND Status LIKE @Status LIMIT @Limit OFFSET @Offset";
        IEnumerable<Book> books = await _db.QueryAsync<Book>(sql, new { UserId = userId, Status = status, Limit = limit, Offset = offset });

        return books;
    }

    public async Task<Book> AddBook(BookDto dto, int userId)
    {
        Console.WriteLine("adding book");
        string sql = @"INSERT INTO Books (Title, Author, Description, ReleaseDate, ImageUrl, Review, Status, UserId) VALUES (@Title, @Author, @Description, @ReleaseDate, @ImageUrl, @Review, @Status, @UserId); SELECT * from Books where BookId = last_insert_rowid()";
        Book book = await _db.QuerySingleAsync<Book>(sql, new
        {
            Title = dto.Title,
            Author = dto.Author,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate.ToString("yyyy-MM-dd"),
            ImageUrl = dto.ImageUrl,
            Review = dto.Review,
            Status = dto.Status,
            UserId = userId
        });
        return book;
    }

    //Returns updated book info or null when error like bookId wrong
    public async Task<Book> UpdateBook(BookDto dto, int userId, int bookId)
    {
        string sql = @"UPDATE Books SET Title = @Title, Author = @Author, Description = @Description, ReleaseDate = @ReleaseDate, ImageUrl = @ImageUrl, Review = @Review, Status = @Status WHERE BookId = @BookId AND UserID = @UserId;
                       SELECT * FROM Books WHERE BookId = @BookId AND UserID = @UserId;";
        using var multiResponse = await _db.QueryMultipleAsync(sql, new
        {
            Title = dto.Title,
            Author = dto.Author,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate.ToString("yyyy-MM-dd"),
            ImageUrl = dto.ImageUrl,
            Review = dto.Review,
            Status = dto.Status,
            UserId = userId,
            BookId = bookId
        });
        Book book = await multiResponse.ReadFirstOrDefaultAsync<Book>();
        return book;
    }
}
