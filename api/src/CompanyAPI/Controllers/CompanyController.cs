using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;
using Serilog;

namespace CompanyApi.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCompaniesAsync()
    {
        var companies = await companyService.GetCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetCompanyById")]
    public async Task<IActionResult> GetCompanyByIdAsync(int id)
    {
        var company = await companyService.GetCompanyByIdAsync(id);

        if (company == null)
        {
            Log.Information("Company with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(company);
    }

    [HttpGet]
    [Route("isin/{isin}")]
    public async Task<IActionResult> GetCompanyByIsinAsync(string isin)
    {
        var company = await companyService.GetCompanyByIsinAsync(isin);

        if (company == null)
        {
            Log.Information("Company with Isin {isin} not found", isin);
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyDTO? createCompanyRequest)
    {
        if (createCompanyRequest == null)
        {
            Log.Information("CreateCompany request is null");
            throw new ArgumentNullException(nameof(createCompanyRequest));
        }

        try
        {
            var companyId = await companyService.CreateCompanyAsync(createCompanyRequest);
            return CreatedAtAction("GetCompanyById", new { id = companyId }, null);
        }
        catch (DuplicateIsinException ex)
        {
            Log.Information("Duplicate Isin error: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidIsinException ex)
        {
            Log.Information("Invalid Isin error: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] UpdateCompanyDTO? updateCompanyRequest)
    {
        if (updateCompanyRequest == null)
        {
            Log.Information("UpdateCompany request is null for ID {Id}", id);
            throw new ArgumentNullException(nameof(updateCompanyRequest));
        }

        try
        {
            await companyService.UpdateCompanyAsync(id, updateCompanyRequest);
            return NoContent();
        }
        catch (CompanyNotFoundException ex)
        {
            Log.Information("Company with ID {Id} not found for update: {Message}", id, ex.Message);
            return NotFound(ex.Message);
        }
        catch (DuplicateIsinException ex)
        {
            Log.Information("Duplicate Isin error during update: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidIsinException ex)
        {
            Log.Information("Invalid Isin error during update: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}
