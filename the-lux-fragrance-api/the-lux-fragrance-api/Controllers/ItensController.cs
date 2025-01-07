using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItensController : ControllerBase
{
    private readonly CatalogoContext _context;

    public ItensController(CatalogoContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItens()
    {
        return await _context.Itens.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _context.Itens.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }
    
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(Item item)
    {
        _context.Itens.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, Item item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.Itens.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.Itens.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}