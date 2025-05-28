using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Violation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers
{
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
        public async Task<IActionResult> GetGroupedViolations()
        {
            var result = await _violationService.GetGroupedViolationsAsync();
            return Ok(result);
        }
    }
} 