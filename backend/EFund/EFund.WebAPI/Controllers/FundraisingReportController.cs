using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/fundraising-reports")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.User)]
public class FundraisingReportController : ControllerBase
{
    private readonly IFundraisingReportService _fundraisingReportService;
    private readonly IValidatorService _validator;

    public FundraisingReportController(IFundraisingReportService fundraisingReportService, IValidatorService validator)
    {
        _fundraisingReportService = fundraisingReportService;
        _validator = validator;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(FundraisingReportDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddReport([FromBody] CreateFundraisingReportDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingReportService.AddAsync(HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(FundraisingReportDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateReport(Guid id, [FromBody] UpdateFundraisingReportDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingReportService.UpdateAsync(id, HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteReport(Guid id)
    {
        var result = await _fundraisingReportService.DeleteAsync(id, HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("{id}/attachments")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddAttachment(Guid id, IFormFileCollection files)
    {
        var result = await _fundraisingReportService.AddAttachmentsAsync(id, HttpContext.GetUserId(), files);
        return result.ToActionResult();
    }

    [HttpDelete("{id}/attachments")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)] 
    public async Task<IActionResult> DeleteAttachments(Guid id, [FromBody] DeleteAttachmentsDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingReportService.DeleteAttachmentsAsync(id, HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }
}