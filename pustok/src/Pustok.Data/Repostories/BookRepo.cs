using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repostories;

public class BookRepo : GenericRepo<Book>, IBookRepo
{
    public BookRepo(AppDBContext context) : base(context)
    {
    }
}
