namespace the_lux_fragrance_api.Models;

public class Vendedor
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public ICollection<Catalogo>? Catalogos { get; set; }
}
