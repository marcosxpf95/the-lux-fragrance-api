namespace the_lux_fragrance_api.Dto;

public record CriarCatalogoDto
{
    public int VendedorId { get; set; }
    public List<int> ItemIds { get; set; } = new List<int>();
}