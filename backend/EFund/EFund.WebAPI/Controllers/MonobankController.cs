using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Monobank;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> AddToken([FromHeader] string token)
    {
        var result = await _monobankService.AddOrUpdateMonobankTokenAsync(HttpContext.GetUserId(), token);
        return result.ToActionResult();
    }

    [HttpGet("jars")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<JarDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> GetJars([FromQuery] string? name)
    {
        var result = await _monobankService.GetJarsAsync(HttpContext.GetUserId(), name);
        return result.ToActionResult();
    }
}