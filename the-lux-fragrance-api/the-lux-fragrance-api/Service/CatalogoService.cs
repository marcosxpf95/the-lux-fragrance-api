using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Service;

public class CatalogoService : ICatalogoService
{
    private readonly ICatalogoRepository _catalogoRepository;

    public CatalogoService(ICatalogoRepository catalogoRepository)
    {
        _catalogoRepository = catalogoRepository;
    }

    public async Task<Catalogo?> AtualizarCatalogoAsync(int id, AtualizarCatalogoDto catalogo)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do catálogo deve ser maior que zero.");

        if (catalogo == null)
            throw new ArgumentNullException(nameof(catalogo));

        var catalogoExistente = await _catalogoRepository.CatalogoExists(id);

        if (!catalogoExistente)
            throw new KeyNotFoundException($"Catálogo com ID {id} não encontrado.");


        if (catalogo.ItemIds != null && catalogo.ItemIds.Any())
        {
            var catalogoItems = MapearCatalogoItens(id, catalogo.ItemIds);

            var catalogoAtualizado = new Catalogo
            {
                CatalogoItens = catalogoItems
            };

            return await _catalogoRepository.AtualizarCatalogoAsync(id, catalogoAtualizado);
        }
        else
        {
            throw new ArgumentException("Nenhum item foi fornecido para o catálogo.");
        }
    }

    public async Task AddCatalogoItem(int catalogoId, int itemId)
    {
        await _catalogoRepository.AddCatalogoItem(catalogoId, itemId);
    }

    public async Task<bool> DeleteCatalogoItem(int catalogoId, int itemId)
    {
        return await _catalogoRepository.DeleteCatalogoItem(catalogoId, itemId);
    }

    public async Task<Catalogo?> CriarCatalogoAsync(CriarCatalogoDto catalogo)
    {
        if (catalogo == null)
            throw new ArgumentNullException(nameof(catalogo));

        var novoCatalogo = new Catalogo
        {
            VendedorId = catalogo.VendedorId,
        };

        var catalogoExistenteParaVendedor = await _catalogoRepository.CatalogoExistsParaVendedor(catalogo.VendedorId);
        
        if (catalogoExistenteParaVendedor)
            throw new InvalidOperationException($"Já existe um catálogo para o vendedor com ID {catalogo.VendedorId}.");

        var catalogoCriado = await _catalogoRepository.CriarCatalogoAsync(novoCatalogo);

        if (catalogoCriado == null)
            throw new InvalidOperationException("Falha ao criar o catálogo.");

        if (catalogo.ItemIds != null && catalogo.ItemIds.Any())
        {
            novoCatalogo.CatalogoItens = MapearCatalogoItens(catalogoCriado.Id, catalogo.ItemIds);
        }
        else
        {
            throw new ArgumentException("Nenhum item foi fornecido para o catálogo.");
        }

        var catalogoAtualizado = await _catalogoRepository.AtualizarCatalogoAsync(catalogoCriado.Id, novoCatalogo);

        return catalogoAtualizado;
    }

    public async Task DeletarCatalogo(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do catálogo deve ser maior que zero.");

        var catalogo = await _catalogoRepository.GetCatalogoByIdAsync(id);

        if (catalogo == null)
            throw new KeyNotFoundException($"Catálogo com ID {id} não encontrado.");

        await _catalogoRepository.DeletarCatalogo(id);
    }

    public Task<Catalogo?> GetCatalogoByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do catálogo deve ser maior que zero.");

        var catalogo = _catalogoRepository.GetCatalogoByIdAsync(id);

        if (catalogo == null)
            throw new KeyNotFoundException($"Catálogo com ID {id} não encontrado.");

        return catalogo;
    }

    public Task<Catalogo?> GetCatalogoByVendedorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do vendedor deve ser maior que zero.");

        var catalogo = _catalogoRepository.GetCatalogoByVendedorIdAsync(id);

        if (catalogo == null)
            throw new KeyNotFoundException($"Catálogo com ID do vendedor {id} não encontrado.");

        return catalogo;
    }

    public async Task<IEnumerable<Catalogo>?> GetCatalogos()
    {
        return await _catalogoRepository.GetCatalogos();
    }
    
    private ICollection<CatalogoItem> MapearCatalogoItens(int catalogoId, List<int> ItemIds)
    {
        return ItemIds.Select(id => new CatalogoItem
        {
            CatalogoId = catalogoId,
            ItemId = id,
        }).ToList();
    }
}