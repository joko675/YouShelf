namespace YouShelf.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description  { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string ImageUrl { get; set; }
    public string Review { get; set; }
    public Status Status { get; set; }
    public int UserId { get; set; }
}
