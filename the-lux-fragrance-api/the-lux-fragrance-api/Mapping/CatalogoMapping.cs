using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Dto;

namespace the_lux_fragrance_api.Mappings;

public static class CatalogoMappingExtensions
{
    public static CatalogoDto ToDto(this Catalogo catalogo)
    {
        return new CatalogoDto
        {
            Id = catalogo.Id,
            NomeVendedor = catalogo.Vendedor?.Nome,
            Itens = catalogo.CatalogoItens?
                .Select(ci => ci.Item?.ToDto())
                .Where(dto => dto != null)
                .Select(dto => dto!)
                .ToList() ?? new List<ItemDto>()
        };
    }
}
