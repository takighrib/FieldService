using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PeopleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/people
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PeopleDTO>>> GetPeople()
    {
        var people = await _context.People
            .Include(p => p.Company) // Include Company to fetch the CompanyName
            .ToListAsync();

        var peopleDTOs = people.Select(p => p.ToDTO()).ToList();

        return Ok(peopleDTOs);
    }

    // GET: api/people/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PeopleDTO>> GetPeople(string id)
    {
        var people = await _context.People
            .Include(p => p.Company) // Include Company to fetch the CompanyName
            .FirstOrDefaultAsync(p => p.Id == id);

        if (people == null)
        {
            return NotFound();
        }

        var peopleDTO = people.ToDTO();
        return Ok(peopleDTO);
    }

    // POST: api/people
    [HttpPost]
    public async Task<ActionResult<PeopleDTO>> PostPeople(PeopleDTO peopleDTO)
    {
        var people = peopleDTO.ToEntity();

        // Optionally check if the CompanyId is valid
        var companyExists = await _context.Companies.AnyAsync(c => c.Id == people.CompanyId);
        if (!companyExists)
        {
            return BadRequest("Invalid Company ID");
        }

        _context.People.Add(people);
        await _context.SaveChangesAsync();

        // Return the created entity with a location header
        return CreatedAtAction(nameof(GetPeople), new { id = people.Id }, peopleDTO);
    }

    // PUT: api/people/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPeople(string id, PeopleDTO peopleDTO)
    {
        if (id != peopleDTO.Id)
        {
            return BadRequest();
        }

        var people = peopleDTO.ToEntity();
        _context.Entry(people).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PeopleExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/people/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePeople(string id)
    {
        var people = await _context.People.FindAsync(id);
        if (people == null)
        {
            return NotFound();
        }

        _context.People.Remove(people);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PeopleExists(string id)
    {
        return _context.People.Any(e => e.Id == id);
    }
}

