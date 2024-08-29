using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using Pustok.Data.Configuration;

namespace Pustok.Data.DAL
{
    public class AppDBContext : DbContext
    {
       public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }  
       
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookImages> BookImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}

