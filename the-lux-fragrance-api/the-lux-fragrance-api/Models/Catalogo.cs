namespace the_lux_fragrance_api.Models;

public class Catalogo
{
    public int Id { get; set; }
    public int VendedorId { get; set; }
    public Vendedor? Vendedor { get; set; }
    public ICollection<CatalogoItem>? CatalogoItens { get; set; }
}