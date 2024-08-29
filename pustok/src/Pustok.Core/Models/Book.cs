namespace Pustok.Core.Models;

public class Book:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
    public double CostPrice { get; set; }
    public double SalePrice { get; set; }
    public int DiscountPercent { get; set; }
    public bool IsAvailable { get; set; }
    public int StockCount { get; set; }
    public string  ProductCode { get; set; }


    public Author Author { get; set; }
    public Genre Genre { get; set; }
    public List<BookImages> BookImages { get; set; }    

}
