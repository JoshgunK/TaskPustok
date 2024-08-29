namespace Pustok.Core.Models;

public class BookImages: BaseEntity
{
    public string  ImageURL { get; set; }
    public int BookId { get; set; }

    public bool? IsPoster { get; set; }
    public Book Book { get; set; }
}
