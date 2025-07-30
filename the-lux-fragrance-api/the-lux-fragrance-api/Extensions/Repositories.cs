using the_lux_fragrance_api.Repository.Interface;

namespace the_lux_fragrance_api.Repository;

public static class Repositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IVendedorRepository, VendedorRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}