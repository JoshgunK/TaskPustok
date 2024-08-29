namespace Pustok.Core.Models;

public class Author:BaseEntity
{
    public string Fullname { get; set; }
    public List<Book> Books { get; set; }
}
