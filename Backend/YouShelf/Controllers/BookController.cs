using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouShelf.Dtos;
using YouShelf.Models;
using YouShelf.Repositories;

namespace YouShelf.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly BookRepository _bookRepo;
    public BookController(BookRepository bookRepo)
    {
        _bookRepo = bookRepo;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<Book>> GetBooksList(string? title, string? author, int? limit, int? offset)
    {
        limit ??= 10;
        offset ??= 0;
        int userId = int.Parse(User.FindFirst("UserId").Value);

        if (title != null) return await _bookRepo.SearchBookTitle(userId, title, limit.Value, offset.Value);
        if (author != null) return await _bookRepo.SearchBookAuthor(userId, author, limit.Value, offset.Value);

        return await _bookRepo.GetBookList(userId, limit.Value, offset.Value);
    }

    [Authorize]
    [HttpPost]
    public async Task<Book> AddBook(BookDto dto)
    {
        int userId = int.Parse(User.FindFirst("UserId").Value);
        return await _bookRepo.AddBook(dto, userId);
    }
}
