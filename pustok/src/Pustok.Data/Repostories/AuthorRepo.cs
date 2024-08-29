using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repostories;

public class AuthorRepo : GenericRepo<Author>, IAuthorRepo
{
    public AuthorRepo(AppDBContext context) : base(context)
    {
    }
}
