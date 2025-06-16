using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace CompanyApi.Controllers;

[ApiController]
[Authorize]
[Route("api/companies")]
public class CompanyController(ICompanyService companyService, ILogger<CompanyController> logger) : ControllerBase
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

        if (company is null)
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

        if (company is null)
        {
            logger.LogInformation("Company with Isin {isin} not found", isin);
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyDTO? createCompanyRequest)
    {
        if (createCompanyRequest is null)
        {
            logger.LogInformation("CreateCompany request is null");
            return BadRequest("Request body cannot be null");
        }

        try
        {
            var companyId = await companyService.CreateCompanyAsync(createCompanyRequest);
            return CreatedAtAction("GetCompanyById", new { id = companyId }, null);
        }
        catch (DuplicateIsinException ex)
        {
            logger.LogInformation("Duplicate Isin error: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidIsinException ex)
        {
            logger.LogInformation("Invalid Isin error: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            logger.LogInformation("Validation error during update: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message, Errors = ex.Errors });
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] UpdateCompanyDTO? updateCompanyRequest)
    {
        if (updateCompanyRequest is null)
        {
            logger.LogInformation("UpdateCompany request is null for ID {Id}", id);
            return BadRequest("Request body cannot be null");
        }

        try
        {
            await companyService.UpdateCompanyAsync(id, updateCompanyRequest);
            return NoContent();
        }
        catch (CompanyNotFoundException ex)
        {
            logger.LogInformation("Company with ID {Id} not found for update: {Message}", id, ex.Message);
            return NotFound(ex.Message);
        }
        catch (DuplicateIsinException ex)
        {
            logger.LogInformation("Duplicate Isin error during update: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidIsinException ex)
        {
            logger.LogInformation("Invalid Isin error during update: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            logger.LogInformation("Validation error during update: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message, Errors = ex.Errors });
        }
    }
}
