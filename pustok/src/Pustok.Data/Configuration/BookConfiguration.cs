using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Core.Models;

namespace Pustok.Data.Configuration;

public class BookConfiguration:IEntityTypeConfiguration<Book>
{
    public void Configure (EntityTypeBuilder<Book> builder)
    {
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(450);
        builder.Property(x => x.ProductCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.SalePrice).IsRequired();
        builder.Property(x => x.CostPrice).IsRequired();

        builder.HasOne(x=>x.Author).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Genre).WithMany(x => x.Books).HasForeignKey(x => x.GenreId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x=>x.BookImages).WithOne(x=>x.Book).HasForeignKey(x=>x.BookId).OnDelete(DeleteBehavior.Cascade);
    }   
}
