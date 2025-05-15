using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Service.Interface;

public interface IItemService 
{
    public Task<IEnumerable<Item>?> GetItens();
    public Task<Item?> GetItemByIdAsync(int id);
    public Task<Item?> CriarItemAsync(Item item);
    public Task<Item?> AtualizarItemAsync(int id, Item item);
    public Task DeletarItem(int id);
}

