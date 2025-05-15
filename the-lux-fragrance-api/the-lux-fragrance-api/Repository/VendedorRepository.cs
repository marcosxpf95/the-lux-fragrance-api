using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Repository;

public class VendedorRepository : IVendedorRepository
{
    private readonly CatalogoContext _context;

    public VendedorRepository(CatalogoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vendedor>?> GetVendedores()
    {
        return await _context.Vendedores.ToListAsync();
    }

    public async Task<Vendedor?> GetVendedorByIdAsync(int id)
    {
        return await _context.Vendedores.FindAsync(id);
    }

    public async Task<Vendedor> CriarVendedorAsync(Vendedor vendedor)
    {
        if (vendedor == null)
            throw new ArgumentNullException(nameof(vendedor));

        await _context.Vendedores.AddAsync(vendedor);
        await _context.SaveChangesAsync();
        return vendedor;
    }

    public Task<Vendedor> AtualizarVendedorAsync(int id, Vendedor vendedor)
    {
        if (id != vendedor.Id)
            throw new ArgumentException("O ID informado não corresponde ao vendedor.");

        var existente = _context.Vendedores.Find(id);
        if (existente == null)
            throw new KeyNotFoundException($"Vendedor com ID {id} não encontrado.");

        existente.Nome = vendedor.Nome;
        existente.Telefone = vendedor.Telefone;
        existente.Email = vendedor.Email;

        _context.Vendedores.Update(existente);
        _context.SaveChanges();

        return Task.FromResult(existente);
    }

    public async Task DeletarVendedor(int id)
    {
        var vendedor = await _context.Vendedores.FindAsync(id);
        if (vendedor != null)
        {
            _context.Vendedores.Remove(vendedor);
            _context.SaveChanges();
        }
    }
}