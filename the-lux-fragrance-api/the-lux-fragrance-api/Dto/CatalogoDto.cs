namespace the_lux_fragrance_api.Dto;


public class CatalogoDto
{
    public int? Id { get; set; }
    public string? NomeVendedor { get; set; }
    public ICollection<ItemDto>? Itens { get; set; } = new List<ItemDto>();
}