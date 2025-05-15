using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;

namespace the_lux_fragrance_api.Repository;

public class ItemRepository : IItemRepository
{
    private readonly CatalogoContext _context;

    public ItemRepository(CatalogoContext context)
    {
        _context = context;
    }

    public async Task<Item> CriarItemAsync(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _context.Itens.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Item?> AtualizarItemAsync(int id, Item item)
    {
        if (id != item.Id)
            throw new ArgumentException("O ID informado não corresponde ao item.");

        var existente = await _context.Itens.FindAsync(id);
        if (existente == null)
            return null;

        existente.Nome = item.Nome;
        existente.Preco = item.Preco;

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task DeletarItem(int id)
    {
        var item = await _context.Itens.FindAsync(id);
        if (item != null)
        {
            _context.Itens.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        return await _context.Itens.FindAsync(id);
    }

    public async Task<IEnumerable<Item>?> GetItens()
    {
        return await _context.Itens.ToListAsync();
    }
}