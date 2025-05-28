using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogoController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;

    public CatalogoController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCatalogos()
    {
        var catalogos = await _catalogoService.GetCatalogos();

        if (catalogos == null || !catalogos.Any())
            return NotFound("Nenhum catálogo encontrado.");

        var response = catalogos.Select(c => c.ToDto()).ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCatalogoById(int id)
    {
        var catalogo = await _catalogoService.GetCatalogoByIdAsync(id);

        if (catalogo == null)
            return NotFound();

        var response = catalogo.ToDto();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CriarCatalogo([FromBody] CriarCatalogoDto criarCatalogoDto)
    {
        if (criarCatalogoDto == null)
            return BadRequest("Catálogo não pode ser nulo.");

        var novoCatalogo = await _catalogoService.CriarCatalogoAsync(criarCatalogoDto);

        if (novoCatalogo == null)
            return BadRequest("Erro ao criar o catálogo.");

        return CreatedAtAction(nameof(GetCatalogoById), new { id = novoCatalogo.Id }, novoCatalogo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCatalogo(int id, [FromBody] AtualizarCatalogoDto atualizarCatalogoDto)
    {
        if (atualizarCatalogoDto == null)
            return BadRequest("Catálogo não pode ser nulo.");

        var catalogoAtualizado = await _catalogoService.AtualizarCatalogoAsync(id, atualizarCatalogoDto);

        if (catalogoAtualizado == null)
            return NotFound();

        return Ok(catalogoAtualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCatalogo(int id)
    {
        var catalogo = await _catalogoService.GetCatalogoByIdAsync(id);

        if (catalogo == null)
            return NotFound();

        await _catalogoService.DeletarCatalogo(id);

        return NoContent();
    }

    [HttpGet("vendedor/{vendedorId}")]
    public async Task<IActionResult> GetCatalogoByVendedorId(int vendedorId)
    {
        var catalogo = await _catalogoService.GetCatalogoByVendedorIdAsync(vendedorId);

        if (catalogo == null)
            return NotFound();

        var response = catalogo.ToDto();

        return Ok(response);
    }
}