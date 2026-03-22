namespace YouShelf.Models;

public class Book
{
    public string Title { get; set; }
    public string Description  { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string ImageUrl { get; set; }
    public string Review { get; set; }
}
