using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Repository.Interface;

public interface ICatalogoRepository
{
    Task<IEnumerable<Catalogo>?> GetCatalogos();
    Task<Catalogo?> GetCatalogoByIdAsync(int id);
    Task<Catalogo?> GetCatalogoByVendedorIdAsync(int id);
    Task<Catalogo> CriarCatalogoAsync(Catalogo catalogo);
    Task<Catalogo?> AtualizarCatalogoAsync(int id, Catalogo catalogo);
    Task<bool> CatalogoExists(int id);
    Task<bool> CatalogoExistsParaVendedor(int vendedorId);
    Task AddCatalogoItem(int catalogoId, int itemId);
    Task<bool> DeleteCatalogoItem(int catalogoId, int itemId);
    Task DeletarCatalogo(int id);
}