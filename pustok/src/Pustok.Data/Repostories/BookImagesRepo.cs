using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repostories;

public class BookImagesRepo : GenericRepo<BookImages>, IBookImagesRepo
{
    public BookImagesRepo(AppDBContext context) : base(context)
    {
    }
}
