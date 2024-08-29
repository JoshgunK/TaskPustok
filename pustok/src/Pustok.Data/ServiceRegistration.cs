using Microsoft.Extensions.DependencyInjection;
using Pustok.Core.Repositories;
using Pustok.Data.Repostories;

namespace Pustok.Data;

public static class ServiceRegistration
{
    public static void AddRepos(this IServiceCollection services)
    {
        services.AddScoped<IGenreRepo, GenreRepo>();
        services.AddScoped<IBookRepo, BookRepo>();
        services.AddScoped<IAuthorRepo, AuthorRepo>();
        services.AddScoped<IBookImagesRepo, BookImagesRepo>();
    }

  
}
