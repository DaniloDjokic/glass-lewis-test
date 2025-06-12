using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers;

[ApiController]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    [HttpGet]
    [Route("api/companies")]
    public async Task<IActionResult> GetCompaniesAsync()
    {
        var companies = await companyService.GetCompaniesAsync();
        return Ok(companies);
    }
}
