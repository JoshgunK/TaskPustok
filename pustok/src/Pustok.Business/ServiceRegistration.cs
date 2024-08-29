using Microsoft.Extensions.DependencyInjection;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;

namespace Pustok.Business;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
            
    }
}
