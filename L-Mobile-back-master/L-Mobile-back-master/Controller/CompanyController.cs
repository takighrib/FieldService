using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CompanyController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Company
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompanies()
    {
        return await _context.Companies
            .Select(company => company.ToDTO())
            .ToListAsync();
    }

    // GET: api/Company/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDTO>> GetCompany(int id)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company == null)
        {
            return NotFound();
        }

        return company.ToDTO();
    }

    // POST: api/Company
    [HttpPost]
    public async Task<ActionResult<CompanyDTO>> PostCompany(CompanyDTO companyDTO)
    {
        if (companyDTO == null)
        {
            return BadRequest("Company data is required.");
        }

        var company = companyDTO.ToEntity();

        if (string.IsNullOrEmpty(company.Name) || string.IsNullOrEmpty(company.Address) || string.IsNullOrEmpty(company.Phone))
        {
            return BadRequest("Name, Address, and Phone are required fields.");
        }

        _context.Companies.Add(company);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }

        return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company.ToDTO());
    }

    // PUT: api/Company/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany(int id, CompanyDTO companyDTO)
    {
        if (id != companyDTO.Id)
        {
            return BadRequest("Company ID mismatch.");
        }

        var existingCompany = await _context.Companies.FindAsync(id);
        if (existingCompany == null)
        {
            return NotFound();
        }

        existingCompany.Name = companyDTO.Name;
        existingCompany.Address = companyDTO.Address;
        existingCompany.Phone = companyDTO.Phone;
        // Update other properties as necessary

        _context.Entry(existingCompany).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompanyExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Company/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Company/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCompanyCount()
    {
        var count = await _context.Companies.CountAsync();
        return Ok(count);
    }

    private bool CompanyExists(int id)
    {
        return _context.Companies.Any(e => e.Id == id);
    }
}
