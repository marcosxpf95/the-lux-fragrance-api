using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Repository;

public interface IVendedorRepository
{
    Task<IEnumerable<Vendedor>?> GetVendedores();
    Task<Vendedor?> GetVendedorByIdAsync(int id);
    Task<Vendedor> CriarVendedorAsync(Vendedor vendedor);
    Task<Vendedor> AtualizarVendedorAsync(int id, Vendedor vendedor);
    Task DeletarVendedor(int id);
}