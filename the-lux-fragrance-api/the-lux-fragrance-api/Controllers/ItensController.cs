using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItensController(IItemService itemService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItens()
    {
        var itens = await itemService.GetItens();

        if (itens == null || !itens.Any())
        {
            return NotFound("Nenhum item encontrado.");
        }

        var itensDto = itens.Select(item => item.ToDto());

        return Ok(itensDto);
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ItemDto>> GetItem(int id)
    {
        var item = await itemService.GetItemByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item.ToDto();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Item>> PostItem(ItemDto item)
    {
        var itemCreated = await itemService.CriarItemAsync(item.ToModel());

        if (itemCreated == null)
        {
            return BadRequest("Erro ao criar o item.");
        }

        return CreatedAtAction(nameof(GetItem), new { id = itemCreated.Id }, itemCreated);
    }
    
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> PutItem(int id, ItemDto item)
    {
        var itemAtualizado = await itemService.AtualizarItemAsync(id, item.ToModel());

        if (itemAtualizado == null)
        {
            return NotFound($"Item com ID {id} não encontrado.");
        }

        return Ok(itemAtualizado); 
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await itemService.GetItemByIdAsync(id);

        if (item == null)
        {
            return NotFound($"Item com ID {id} não encontrado.");
        }

        await itemService.DeletarItem(item.Id);

        return NoContent();
    }
}