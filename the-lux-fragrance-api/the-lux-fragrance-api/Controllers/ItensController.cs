using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItensController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItensController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItens()
    {
        var itens = await _itemService.GetItens();

        if (itens == null || !itens.Any())
        {
            return NotFound("Nenhum item encontrado.");
        }

        var itensDto = itens.Select(item => item.ToDto());

        return Ok(itensDto);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(int id)
    {
        var item = await _itemService.GetItemByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item.ToDto();
    }
    
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(ItemDto item)
    {
        var itemCreated = await _itemService.CriarItemAsync(item.ToModel());

        if (itemCreated == null)
        {
            return BadRequest("Erro ao criar o item.");
        }

        return CreatedAtAction(nameof(GetItem), new { id = itemCreated.Id }, itemCreated);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, ItemDto item)
    {
        var itemAtualizado = await _itemService.AtualizarItemAsync(id, item.ToModel());

        if (itemAtualizado == null)
        {
            return NotFound($"Item com ID {id} não encontrado.");
        }

        return Ok(itemAtualizado); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _itemService.GetItemByIdAsync(id);

        if (item == null)
        {
            return NotFound($"Item com ID {id} não encontrado.");
        }

        await _itemService.DeletarItem(item.Id);

        return NoContent();
    }
}