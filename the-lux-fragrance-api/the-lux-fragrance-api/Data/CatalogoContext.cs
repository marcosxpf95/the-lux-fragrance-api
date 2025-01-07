using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Data;

public class CatalogoContext : DbContext
{
    public CatalogoContext(DbContextOptions<CatalogoContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Itens { get; set; }  // Define a tabela 'Itens'
}