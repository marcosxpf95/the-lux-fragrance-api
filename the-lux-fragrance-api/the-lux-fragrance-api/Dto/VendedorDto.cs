namespace the_lux_fragrance_api.Dto;

public record VendedorDto
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
}