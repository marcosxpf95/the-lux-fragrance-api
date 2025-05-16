namespace the_lux_fragrance_api.Dto;

public record ItemDto
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemPerfume { get; set; }
    public string? ImagemReferencia { get; set; }
    public string? Categoria { get; set; }
}