using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Service;

public class ItemService(IItemRepository itemRepository, ICatalogoService catalogoService)
    : IItemService
{
    private const int CatalogoId = 35;
    private readonly IItemRepository _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    private readonly ICatalogoService _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));

    public async Task<Item?> AtualizarItemAsync(int id, Item item)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var existente = await _itemRepository.GetItemByIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"Item com ID {id} não encontrado.");

        return await _itemRepository.AtualizarItemAsync(id, item);
    }

    public async Task<Item?> CriarItemAsync(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (string.IsNullOrWhiteSpace(item.Nome))
            throw new ArgumentException("O nome do item é obrigatório.");

        if (item.Preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.");


        var itemCriado = await _itemRepository.CriarItemAsync(item);
        
        if (itemCriado.Id > 0)
        {
            await _catalogoService.AddCatalogoItem(CatalogoId, itemCriado.Id);
        }

        return itemCriado;
    }

    public async Task DeletarItem(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        var item = await _itemRepository.GetItemByIdAsync(id);
        if (item == null)
            throw new KeyNotFoundException($"Item com ID {id} não encontrado.");

        var itemDeletado = _itemRepository.DeletarItem(id).Result;

        if (itemDeletado)
        {
            await _catalogoService.DeleteCatalogoItem(CatalogoId, id);
        }
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        var item = await _itemRepository.GetItemByIdAsync(id);
        if (item == null)
            throw new KeyNotFoundException($"Item com ID {id} não encontrado.");

        return item;
    }

    public async Task<IEnumerable<Item>?> GetItens()
    {
        return await _itemRepository.GetItens();
    }
}