using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;

namespace the_lux_fragrance_api.Repository;

public class CatalogoRepository : ICatalogoRepository
{
    private readonly CatalogoContext _context;

    public CatalogoRepository(CatalogoContext context)
    {
        _context = context;
    }

    public async Task<Catalogo?> AtualizarCatalogoAsync(int id, Catalogo catalogo)
    {
        var catalogoParaAtualizar = await _context.Catalogos.FindAsync(id);

        if (catalogoParaAtualizar == null)
            return null;


        catalogoParaAtualizar.CatalogoItens = catalogo.CatalogoItens;

        await _context.SaveChangesAsync();

        return catalogoParaAtualizar;
    }

    public Task<bool> CatalogoExists(int id)
    {
        return _context.Catalogos.AnyAsync(c => c.Id == id);
    }

    public Task<bool> CatalogoExistsParaVendedor(int vendedorId)
    {
        return _context.Catalogos.AnyAsync(c => c.VendedorId == vendedorId);
    }

    public Task<Catalogo> CriarCatalogoAsync(Catalogo catalogo)
    {
        if (catalogo == null)
            throw new ArgumentNullException(nameof(catalogo));

        _context.Catalogos.Add(catalogo);
        _context.SaveChanges();

        return Task.FromResult(catalogo);
    }

    public async Task DeletarCatalogo(int id)
    {
        var catalogo = await _context.Catalogos.FindAsync(id);

        if (catalogo != null)
        {
            _context.Catalogos.Remove(catalogo);
            _context.SaveChanges();
        }
    }

    public async Task<Catalogo?> GetCatalogoByIdAsync(int id)
    {
        return await _context.Catalogos
            .Include(c => c.Vendedor)
            .Include(c => c.CatalogoItens!)
            .ThenInclude(ci => ci.Item)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Catalogo?> GetCatalogoByVendedorIdAsync(int id)
    {
        return await _context.Catalogos
            .Include(c => c.Vendedor)
            .Include(x => x.CatalogoItens!)
            .ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(c => c.VendedorId == id);
    }

    public async Task<IEnumerable<Catalogo>?> GetCatalogos()
    {
        return await _context.Catalogos
            .Include(c => c.Vendedor)
            .Include(c => c.CatalogoItens!)
            .ThenInclude(ci => ci.Item)
            .ToListAsync();
    }
}