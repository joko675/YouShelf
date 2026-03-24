using Microsoft.AspNetCore.Mvc;
using YouShelf.Dtos;
using YouShelf.Repositories;

namespace YouShelf.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserRepository _userRepo;
    public AuthController(IConfiguration config, UserRepository userRepo)
    {
        _config = config;
        _userRepo = userRepo;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UserAuthDto dto)
    {
        int failOrNewId = await _userRepo.CreateUser(dto);
        if (failOrNewId == 0) return BadRequest(new {Success = false, UserId = (int?)null, ErrorMessage = "User with this username already exists" });
        else return Ok(new { Success = true, UserId = failOrNewId, ErrorMessage = (string?)null });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserAuthDto dto)
    {
        string token = await _userRepo.Login(dto);
        if (token == "") return BadRequest(new {Success = false, token = (string?)null, ErrorMessage = "Wrong username or password" });
        return Ok( new { Success = true, Token = token, ErrorMessage = (string?)null });
    }
}
