using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Dto;

namespace the_lux_fragrance_api.Mappings;

public static class ItemMappingExtensions
{
    public static ItemDto ToDto(this Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Nome = item.Nome,
            Descricao = item.Descricao,
            Preco = item.Preco,
            ImagemPerfume = item.ImagemPerfume,
            ImagemReferencia = item.ImagemReferencia,
            Categoria = item.Categoria
        };
    }

    public static Item ToModel(this ItemDto dto)
    {
        return new Item
        {
            Id = dto.Id ?? 0,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            ImagemPerfume = dto.ImagemPerfume,
            ImagemReferencia = dto.ImagemReferencia,
            Categoria = dto.Categoria
        };
    }

    public static void UpdateModel(this Item item, ItemDto dto)
    {
        item.Nome = dto.Nome;
        item.Descricao = dto.Descricao;
        item.Preco = dto.Preco;
        item.ImagemPerfume = dto.ImagemPerfume;
        item.ImagemReferencia = dto.ImagemReferencia;
        item.Categoria = dto.Categoria;
    }
}
