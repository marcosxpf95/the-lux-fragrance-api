using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendedorController : ControllerBase
{
    private readonly IVendedorService _vendedorService;

    public VendedorController(IVendedorService vendedorService)
    {
        _vendedorService = vendedorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVendedores()
    {
        var vendedores = await _vendedorService.GetVendedores();

        if (vendedores == null || !vendedores.Any())
            return NotFound("Nenhum vendedor encontrado.");

        var vendedoresDto = vendedores.Select(v => v.ToDto());

        return Ok(vendedoresDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendedorById(int id)
    {
        var vendedor = await _vendedorService.GetVendedorByIdAsync(id);

        if (vendedor == null)
            return NotFound();

        return Ok(vendedor.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> CriarVendedor([FromBody] VendedorDto vendedor)
    {
        if (vendedor == null)
            return BadRequest("Vendedor não pode ser nulo.");

        var novoVendedor = await _vendedorService.CriarVendedorAsync(vendedor.ToModel());

        if (novoVendedor == null)
            return BadRequest("Erro ao criar o vendedor.");

        return CreatedAtAction(nameof(GetVendedorById), new { id = novoVendedor.Id }, novoVendedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarVendedor(int id, [FromBody] VendedorDto vendedor)
    {
        if (vendedor == null)
            return BadRequest("Vendedor não pode ser nulo.");

        var vendedorAtualizado = await _vendedorService.AtualizarVendedorAsync(id, vendedor.ToModel());

        if (vendedorAtualizado == null)
            return NotFound();

        return Ok(vendedorAtualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarVendedor(int id)
    {
        var vendedor = await _vendedorService.GetVendedorByIdAsync(id);
        if (vendedor == null)
            return NotFound();

        await _vendedorService.DeletarVendedor(id);
        return NoContent();
    }
}