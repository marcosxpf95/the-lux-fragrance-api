using the_lux_fragrance_api.Helpers;
using the_lux_fragrance_api.Service;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Repository;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IVendedorService, VendedorService>();
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<JwtService>();

        return services;
    }
}