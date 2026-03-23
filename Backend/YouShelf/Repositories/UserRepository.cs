using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using YouShelf.Dtos;
using YouShelf.Models;
using YouShelf.Services;

namespace YouShelf.Repositories;

public class UserRepository
{
    private readonly IDbConnection _db;
    private readonly JwtService _jwtService;

    public UserRepository(IDbConnection db, JwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    //Returns 0 when username already exist, new user id if created
    public async Task<int> CreateUser(UserAuthDto dto)
    {
        if (UsernameExist(dto.Username)) return 0;

        var passwordHasher = new PasswordHasher<User>();

        User user = new User();
        user.Username = dto.Username;
        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

        string sql = @"INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash); SELECT last_insert_rowid();";

        return await _db.ExecuteScalarAsync<int>(sql, user);
    }
    public async Task<string> Login(UserAuthDto dto)
    {
        if (!UsernameExist(dto.Username)) return "";

        string sql = @"SELECT * FROM Users WHERE Username = @Username";
        User user = await _db.QueryFirstAsync<User>(sql, new { Username = dto.Username});

        var passwordHasher = new PasswordHasher<User>();

        if (dto.Username == user.Username && passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Success)
        {
            return _jwtService.GenerateToken(user.Username, user.UserId);
        }
        else return "";

    }

    public bool UsernameExist(string username)
    {
        string sql = @"SELECT * FROM Users WHERE Username = @Username";
        User exist = _db.QueryFirstOrDefault<User>(sql, new { Username = username });

        if (exist == null) return false;
        return true;
    }
}
