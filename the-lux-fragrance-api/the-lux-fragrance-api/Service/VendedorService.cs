using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Service;

public class VendedorService : IVendedorService
{
    private readonly IVendedorRepository _vendedorRepository;

    public VendedorService(IVendedorRepository vendedorRepository)
    {
        _vendedorRepository = vendedorRepository ?? throw new ArgumentNullException(nameof(vendedorRepository));
    }

    public async Task<Vendedor?> AtualizarVendedorAsync(int id, Vendedor vendedor)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        if (vendedor == null)
            throw new ArgumentNullException(nameof(vendedor));

        var existente = await _vendedorRepository.GetVendedorByIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"Vendedor com ID {id} não encontrado.");

        return await _vendedorRepository.AtualizarVendedorAsync(id, vendedor);
    }

    public Task<Vendedor> CriarVendedorAsync(Vendedor vendedor)
    {
        if (vendedor == null)
            throw new ArgumentNullException(nameof(vendedor));

        if (string.IsNullOrWhiteSpace(vendedor.Nome))
            throw new ArgumentException("O nome do vendedor é obrigatório.");

        if (string.IsNullOrWhiteSpace(vendedor.Telefone))
            throw new ArgumentException("O telefone do vendedor é obrigatório.");

        if (string.IsNullOrWhiteSpace(vendedor.Email))
            throw new ArgumentException("O email do vendedor é obrigatório.");

        return _vendedorRepository.CriarVendedorAsync(vendedor);
    }

    public Task DeletarVendedor(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        var vendedor = _vendedorRepository.GetVendedorByIdAsync(id);

        if (vendedor == null)
            throw new KeyNotFoundException($"Vendedor com ID {id} não encontrado.");

        return _vendedorRepository.DeletarVendedor(id);
    }

    public Task<Vendedor?> GetVendedorByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.", nameof(id));

        var vendedor = _vendedorRepository.GetVendedorByIdAsync(id);

        if (vendedor == null)
            throw new KeyNotFoundException($"Vendedor com ID {id} não encontrado.");

        return vendedor;
    }

    public Task<IEnumerable<Vendedor>?> GetVendedores()
    {
        return _vendedorRepository.GetVendedores();
    }
}
