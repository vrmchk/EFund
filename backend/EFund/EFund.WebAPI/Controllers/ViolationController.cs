using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Violation;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/violations")]
public class ViolationController : ControllerBase
{
    private readonly IViolationService _violationService;

    public ViolationController(IViolationService violationService)
    {
        _violationService = violationService;
    }

    [HttpGet("groups")]
    [SwaggerOperation(
        Summary = "Get grouped violations",
        Description = "Returns a list of violation groups, each with its violations."
    )]
    [SwaggerResponse(200, "List of grouped violations", typeof(List<ViolationGroupDTO>))]
    public async Task<IActionResult> GetGroupedViolations([FromQuery] bool withDeleted = false)
    {
        var result = await _violationService.GetGroupedViolationsAsync(withDeleted);
        return Ok(result);
    }
    
    [HttpGet("{violationId}")]
    [SwaggerOperation(
        Summary = "Get violation by Id",
        Description = "Returns an extended violation by its Id."
    )]
    [SwaggerResponse(200, "Extended violation details", typeof(ViolationDTO))]
    [SwaggerResponse(400, "Invalid violation Id", typeof(ErrorDTO))]
    public async Task<IActionResult> GetViolationById(Guid violationId)
    {
        var result = await _violationService.GetByIdAsync(violationId);
        return result.ToActionResult();
    }
}