using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/monobank")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
public class MonobankController : ControllerBase
{
    private readonly IMonobankService _monobankService;

    public MonobankController(IMonobankService service)
    {
        _monobankService = service;
    }

    [HttpPost("link-token")]
    public async Task<IActionResult> AddToken([FromHeader] string token)
    {
        var result = await _monobankService.AddOrUpdateMonobankTokenAsync(HttpContext.GetUserId(), token);
        return result.ToActionResult();
    }

    [HttpGet("jars")]
    public async Task<IActionResult> GetJars()
    {
        var result = await _monobankService.GetJarsAsync(HttpContext.GetUserId());
        return result.ToActionResult();
    }
}