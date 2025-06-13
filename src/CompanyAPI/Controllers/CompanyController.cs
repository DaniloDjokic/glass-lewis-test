using Application.Services;
using Application.Requests;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequest? createCompanyRequest)
    {
        if (createCompanyRequest == null)
        {
            throw new ArgumentNullException(nameof(createCompanyRequest));
        }

        var companyId = await companyService.CreateCompanyAsync(createCompanyRequest);
        return CreatedAtAction("GetCompanyById", new { id = companyId }, null);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] UpdateCompanyRequest? updateCompanyRequest)
    {
        if (updateCompanyRequest == null)
        {
            throw new ArgumentNullException(nameof(updateCompanyRequest));
        }

        //TODO: Add try catch with proper return types

        await companyService.UpdateCompanyAsync(id, updateCompanyRequest);
        return NoContent();
    }
}
