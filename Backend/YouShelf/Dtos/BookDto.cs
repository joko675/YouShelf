using System.ComponentModel.DataAnnotations;

namespace YouShelf.Dtos;

public class BookDto
{
    [Required]
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string ImageUrl { get; set; } = "https://img.freepik.com/free-vector/red-text-book-closed-icon_18591-82397.jpg?semt=ais_hybrid&w=740&q=80";
    public string Review { get; set; }
    [Required]
    public Status? Status { get; set; }
}
