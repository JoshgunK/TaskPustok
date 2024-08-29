using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repostories;

public class GenreRepo : GenericRepo<Genre>, IGenreRepo
{
    public GenreRepo(AppDBContext context) : base(context) { }

}
