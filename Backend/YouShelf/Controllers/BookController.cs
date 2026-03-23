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
    public async Task<IEnumerable<Book>> GetBooksList()
    {
        int userId = int.Parse(User.FindFirst("UserId").Value);
        IEnumerable<Book> books = await _bookRepo.GetBookList(userId);
        return books;
    }

    [Authorize]
    [HttpPost]
    public async Task<Book> AddBook(BookDto dto)
    {
        int userId = int.Parse(User.FindFirst("UserId").Value);
        return await _bookRepo.AddBook(dto, userId);
    }
}
