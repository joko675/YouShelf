using Dapper;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Data;
using System.Text;
using YouShelf.Repositories;
using YouShelf.Services;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtOptions =>
{
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),

        ValidateAudience = true,
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")))
    };
    jwtOptions.MapInboundClaims = false;
});

string connectionString = "Data Source=app.db";
builder.Services.AddScoped<IDbConnection>(sp => new SqliteConnection(connectionString));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddSingleton<JwtService>();
SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

builder.Services.AddControllers();

var app = builder.Build();

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var sql = connection.CreateCommand();
    sql.CommandText = @"CREATE TABLE IF NOT EXISTS Users (UserId INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL UNIQUE, PasswordHash TEXT NOT NULL);";
    sql.ExecuteNonQuery();
    sql.CommandText = @"CREATE TABLE IF NOT EXISTS BOOKS (BookId INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL, Description TEXT, ReleaseDate TEXT, ImageUrl TEXT NOT NULL, Review TEXT, UserId INTEGER NOT NULL);";
    sql.ExecuteNonQuery();
    Console.WriteLine("DB Table created");
}

    // Configure the HTTP request pipeline.

    app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
