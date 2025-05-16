namespace the_lux_fragrance_api.Dto;

public record AtualizarCatalogoDto
{
    public List<int> ItemIds { get; set; } = new List<int>();
}