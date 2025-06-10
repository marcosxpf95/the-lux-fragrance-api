using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Data;

public class CatalogoContext : DbContext
{
    public CatalogoContext(DbContextOptions<CatalogoContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Item> Itens { get; set; }
    public DbSet<Vendedor> Vendedores { get; set; }
    public DbSet<Catalogo> Catalogos { get; set; }
    public DbSet<CatalogoItem> CatalogoItens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(entity => entity.Email).IsUnique();
        
        modelBuilder.Entity<CatalogoItem>()
            .HasKey(ci => new { ci.CatalogoId, ci.ItemId });

        modelBuilder.Entity<CatalogoItem>()
            .HasOne(ci => ci.Catalogo)
            .WithMany(c => c.CatalogoItens)
            .HasForeignKey(ci => ci.CatalogoId);

        modelBuilder.Entity<CatalogoItem>()
            .HasOne(ci => ci.Item)
            .WithMany(i => i.CatalogoItens)
            .HasForeignKey(ci => ci.ItemId);
    }
}