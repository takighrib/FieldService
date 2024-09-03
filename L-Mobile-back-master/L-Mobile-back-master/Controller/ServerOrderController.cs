using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ServiceOrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiceOrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ServiceOrder
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceOrderDTO>>> GetServiceOrders()
    {
        var serviceOrders = await _context.ServiceOrders
            .Include(so => so.Company)
            .Include(so => so.User) // Include the user
            .Include(so => so.Articles)
            .Include(so => so.Dispatchers)
            .Select(so => new ServiceOrderDTO
            {
                Id = so.Id,
                CompanyId = so.CompanyId,
                UserId = so.UserId, // Single UserId
                ArticleIds = so.ArticleIds,
                Status = so.GetServiceOrderStatusName(), // Use method in ServiceOrder
                Progress = so.Progress,
                CreatedAt = so.CreatedAt,
                UpdatedAt = so.UpdatedAt,
                Dispatchers = so.Dispatchers.Select(d => new DispatcherDTO
                {
                    Id = d.Id,
                    TechniciansIds = d.TechniciansIds,
                    DispatchDate = d.DispatchDate,
                    Message = d.Message,
                }).ToList()
            })
            .ToListAsync();

        return Ok(serviceOrders);
    }

    // GET: api/ServiceOrder/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceOrderDTO>> GetServiceOrder(int id)
    {
        var serviceOrder = await _context.ServiceOrders
            .Include(so => so.Company)
            .Include(so => so.User) // Include the user
            .Include(so => so.Articles)
            .Include(so => so.Dispatchers)
            .Where(so => so.Id == id)
            .Select(so => new ServiceOrderDTO
            {
                Id = so.Id,
                CompanyId = so.CompanyId,
                UserId = so.UserId, // Single UserId
                ArticleIds = so.ArticleIds,
                Status = so.GetServiceOrderStatusName(), // Use method in ServiceOrder
                Progress = so.Progress,
                CreatedAt = so.CreatedAt,
                UpdatedAt = so.UpdatedAt,
                Dispatchers = so.Dispatchers.Select(d => new DispatcherDTO
                {
                    Id = d.Id,
                    TechniciansIds = d.TechniciansIds,
                    DispatchDate = d.DispatchDate,
                    Message = d.Message,
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (serviceOrder == null)
        {
            return NotFound("Service order not found.");
        }

        return Ok(serviceOrder);
    }

    // POST: api/ServiceOrder
    [HttpPost]
    public async Task<ActionResult<ServiceOrder>> PostServiceOrder(ServiceOrderDTO dto)
    {
        var serviceOrder = new ServiceOrder
        {
            CompanyId = dto.CompanyId,
            UserId = dto.UserId, // Assign UserId
            ArticleIds = dto.ArticleIds,
            Status = Enum.Parse<ServiceOrderStatus>(dto.Status), // Convert string to enum
            Progress = dto.Progress,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            Dispatchers = dto.Dispatchers.Select(d => new Dispatcher
            {
                Id = d.Id,
                TechniciansIds = d.TechniciansIds,
                DispatchDate = d.DispatchDate,
                Message = d.Message,
            }).ToList()
        };

        _context.ServiceOrders.Add(serviceOrder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServiceOrder), new { id = serviceOrder.Id }, serviceOrder);
    }

    // PUT: api/ServiceOrder/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutServiceOrder(int id, [FromBody] ServiceOrderDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("The ID in the URL does not match the ID in the body.");
        }

        var serviceOrder = await _context.ServiceOrders
            .Include(so => so.Dispatchers)
            .FirstOrDefaultAsync(so => so.Id == id);

        if (serviceOrder == null)
        {
            return NotFound("Service order not found.");
        }

        // Update properties of service order
        serviceOrder.CompanyId = dto.CompanyId;
        serviceOrder.UserId = dto.UserId; // Update UserId
        serviceOrder.ArticleIds = dto.ArticleIds;
        serviceOrder.Status = Enum.Parse<ServiceOrderStatus>(dto.Status); // Convert string to enum
        serviceOrder.Progress = dto.Progress;
        serviceOrder.CreatedAt = dto.CreatedAt;
        serviceOrder.UpdatedAt = dto.UpdatedAt;

        // Update dispatchers
        _context.Dispatchers.RemoveRange(serviceOrder.Dispatchers);
        serviceOrder.Dispatchers = dto.Dispatchers.Select(d => new Dispatcher
        {
            Id = d.Id,
            TechniciansIds = d.TechniciansIds,
            DispatchDate = d.DispatchDate,
            Message = d.Message,
        }).ToList();

        _context.Entry(serviceOrder).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok("Service order updated successfully.");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceOrderExists(id))
            {
                return NotFound("Service order not found due to concurrency issue.");
            }
            else
            {
                return StatusCode(500, "Concurrency error while updating the service order.");
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    // DELETE: api/ServiceOrder/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceOrder(int id)
    {
        var serviceOrder = await _context.ServiceOrders
            .Include(so => so.Dispatchers)
            .FirstOrDefaultAsync(so => so.Id == id);

        if (serviceOrder == null)
        {
            return NotFound("Service order not found.");
        }

        _context.Dispatchers.RemoveRange(serviceOrder.Dispatchers);
        _context.ServiceOrders.Remove(serviceOrder);

        try
        {
            await _context.SaveChangesAsync();
            return Ok("Service order deleted successfully.");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    [HttpGet("ByUser/{userId}")]
    public async Task<ActionResult<IEnumerable<ServiceOrderDTO>>> GetServiceOrdersByUserId(string userId)
    {
        var serviceOrders = await _context.ServiceOrders
            .Where(so => so.UserId == userId)
            .Include(so => so.Dispatchers)
            .Select(so => new ServiceOrderDTO
            {
                Id = so.Id,
                CompanyId = so.CompanyId,
                UserId = so.UserId, // Filter by this UserId
                ArticleIds = so.ArticleIds,
                Status = so.GetServiceOrderStatusName(), // Convert the status to string
                Progress = so.Progress,
                CreatedAt = so.CreatedAt,
                UpdatedAt = so.UpdatedAt,
                Dispatchers = so.Dispatchers.Select(d => new DispatcherDTO
                {
                    Id = d.Id,
                    TechniciansIds = d.TechniciansIds,
                    DispatchDate = d.DispatchDate,
                    Message = d.Message,
                }).ToList()
            })
            .ToListAsync();

        if (!serviceOrders.Any())
        {
            return NotFound($"No service orders found for user with ID {userId}.");
        }

        return Ok(serviceOrders);
    }


    private bool ServiceOrderExists(int id)
    {
        return _context.ServiceOrders.Any(e => e.Id == id);
    }
}
