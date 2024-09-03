using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DispatcherController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DispatcherController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Dispatcher
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MyDispatcherDTO>>> GetDispatchers()
    {
        var dispatchers = await _context.Dispatchers
            .Select(d => new MyDispatcherDTO
            {
                Id = d.Id,
                TechniciansIds = d.TechniciansIds,
                DispatchDate = d.DispatchDate,
                Message = d.Message,
                ServiceOrderId = d.ServiceOrderId
            })
            .ToListAsync();

        return Ok(dispatchers);
    }

    // GET: api/Dispatcher/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MyDispatcherDTO>> GetDispatcher(int id)
    {
        var dispatcher = await _context.Dispatchers
            .Where(d => d.Id == id)
            .Select(d => new MyDispatcherDTO
            {
                Id = d.Id,
                TechniciansIds = d.TechniciansIds,
                DispatchDate = d.DispatchDate,
                Message = d.Message,
                ServiceOrderId = d.ServiceOrderId
            })
            .FirstOrDefaultAsync();

        if (dispatcher == null)
        {
            return NotFound("Dispatcher not found.");
        }

        return Ok(dispatcher);
    }

    // POST: api/Dispatcher
    [HttpPost]
    public async Task<ActionResult<MyDispatcherDTO>> PostDispatcher(MyDispatcherDTO dto)
    {
        var serviceOrderExists = await _context.ServiceOrders.AnyAsync(so => so.Id == dto.ServiceOrderId);

        if (!serviceOrderExists)
        {
            return NotFound("ServiceOrderId does not exist.");
        }

        var dispatcher = new Dispatcher
        {
            TechniciansIds = dto.TechniciansIds,
            DispatchDate = dto.DispatchDate,
            Message = dto.Message,
            ServiceOrderId = dto.ServiceOrderId
        };

        _context.Dispatchers.Add(dispatcher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDispatcher), new { id = dispatcher.Id }, dto);
    }

    // PUT: api/Dispatcher/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDispatcher(int id, MyDispatcherDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("The ID in the URL does not match the ID in the body.");
        }

        var dispatcher = await _context.Dispatchers.FindAsync(id);

        if (dispatcher == null)
        {
            return NotFound("Dispatcher not found.");
        }

        var serviceOrderExists = await _context.ServiceOrders.AnyAsync(so => so.Id == dto.ServiceOrderId);

        if (!serviceOrderExists)
        {
            return NotFound("ServiceOrderId does not exist.");
        }

        dispatcher.TechniciansIds = dto.TechniciansIds;
        dispatcher.DispatchDate = dto.DispatchDate;
        dispatcher.Message = dto.Message;
        dispatcher.ServiceOrderId = dto.ServiceOrderId;

        _context.Entry(dispatcher).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok("Dispatcher updated successfully.");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DispatcherExists(id))
            {
                return NotFound("Dispatcher not found due to concurrency issue.");
            }
            else
            {
                return StatusCode(500, "Concurrency error while updating the dispatcher.");
            }
        }
    }

    // DELETE: api/Dispatcher/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDispatcher(int id)
    {
        var dispatcher = await _context.Dispatchers.FindAsync(id);

        if (dispatcher == null)
        {
            return NotFound("Dispatcher not found.");
        }

        _context.Dispatchers.Remove(dispatcher);
        await _context.SaveChangesAsync();

        return Ok("Dispatcher deleted successfully.");
    }

    [HttpGet("ByTechnician/{technicianId}")]
    public async Task<ActionResult<IEnumerable<DispatcherDTO>>> GetDispatchersByTechnicianId(string technicianId)
    {
        var dispatchers = await _context.Dispatchers
            .Where(d => d.TechniciansIds.Contains(technicianId))
            .Select(d => new MyDispatcherDTO
            {
                Id = d.Id,
                TechniciansIds = d.TechniciansIds,
                DispatchDate = d.DispatchDate,
                Message = d.Message,
                ServiceOrderId = d.ServiceOrderId
            })
            .ToListAsync();

        if (!dispatchers.Any())
        {
            return NotFound($"No dispatchers found for technician with ID {technicianId}.");
        }

        return Ok(dispatchers);
    }


    private bool DispatcherExists(int id)
    {
        return _context.Dispatchers.Any(e => e.Id == id);
    }
}
