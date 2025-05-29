using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Notification;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IValidatorService _validatorService;

    public NotificationController(INotificationService notificationService, IValidatorService validatorService)
    {
        _notificationService = notificationService;
        _validatorService = validatorService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Shared")]
    public async Task<IActionResult> GetList([FromQuery] bool withRead = false)
    {
        var userId = HttpContext.GetUserId();
        var notifications = await _notificationService.GetListAsync(userId, withRead);
        return Ok(notifications);
    }

    [HttpPost("{id}/read")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Shared")]
    public async Task<IActionResult> SetIsRead(Guid id)
    {
        await _notificationService.SetIsRead(id);
        return NoContent();
    }

    [HttpPost("batch-read")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Shared")]
    public async Task<IActionResult> BatchSetIsRead([FromBody] BatchSetNotificationIsReadDTO dto)
    {
        var validationResult = await _validatorService.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        await _notificationService.BatchSetIsRead(dto);
        return NoContent();
    }
}