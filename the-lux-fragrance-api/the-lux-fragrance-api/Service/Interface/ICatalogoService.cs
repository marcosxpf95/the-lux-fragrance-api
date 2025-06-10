using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Service.Interface;

public interface ICatalogoService
{
    Task<IEnumerable<Catalogo>?> GetCatalogos();
    Task<Catalogo?> GetCatalogoByIdAsync(int id);
    Task<Catalogo?> GetCatalogoByVendedorIdAsync(int id);
    Task<Catalogo?> CriarCatalogoAsync(CriarCatalogoDto catalogo);
    Task<Catalogo?> AtualizarCatalogoAsync(int id, AtualizarCatalogoDto catalogo);
    Task AddCatalogoItem(int catalogoId, int itemId);
    Task<bool> DeleteCatalogoItem(int catalogoId, int itemId);
    Task DeletarCatalogo(int id);
}