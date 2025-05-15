namespace the_lux_fragrance_api.Models;

public class CatalogoItem
{
    public int CatalogoId { get; set; }
    public Catalogo? Catalogo { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }
}