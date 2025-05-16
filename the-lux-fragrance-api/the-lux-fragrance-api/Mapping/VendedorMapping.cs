using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Dto;

namespace the_lux_fragrance_api.Mappings;

public static class VendedorMappingExtensions
{
    public static VendedorDto ToDto(this Vendedor vendedor)
    {
        return new VendedorDto
        {
            Id = vendedor.Id,
            Nome = vendedor.Nome,
            Email = vendedor.Email,
            Telefone = vendedor.Telefone
        };
    }

    public static Vendedor ToModel(this VendedorDto dto)
    {
        return new Vendedor
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone
        };
    }

    public static void UpdateModel(this Vendedor vendedor, VendedorDto dto)
    {
        vendedor.Nome = dto.Nome;
        vendedor.Email = dto.Email;
        vendedor.Telefone = dto.Telefone;
    }
}
