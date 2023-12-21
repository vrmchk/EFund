using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Fundraising;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/fundraisings")]
public class FundraisingController : ControllerBase
{
    private readonly IFundraisingService _fundraisingService;
    private readonly IValidatorService _validator;

    public FundraisingController(IFundraisingService fundraisingService, IValidatorService validator)
    {
        _fundraisingService = fundraisingService;
        _validator = validator;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFundraisingDTO dto, [FromQuery] PaginationDTO pagination)
    {
        var validationResult = await _validator.ValidateAsync(pagination);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingService.Search(dto, pagination, HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fundraisingService.GetByIdAsync(id, HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.User)]
    public async Task<IActionResult> Add([FromBody] CreateFundraisingDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingService.AddAsync(HttpContext.GetUserId(), dto, HttpContext.GetApiUrl());
        return result.Match<IActionResult>(
            Right: fundraising => CreatedAtAction(nameof(GetById), new { id = fundraising.Id }, fundraising),
            Left: BadRequest
        );
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.User)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFundraisingDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _fundraisingService.UpdateAsync(id, HttpContext.GetUserId(), dto, HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = HttpContext.IsAdmin()
            ? await _fundraisingService.DeleteAsync(id)
            : await _fundraisingService.DeleteAsync(id, HttpContext.GetUserId());

        return result.ToActionResult();
    }

    [HttpPost("{id}/upload-avatar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.User)]
    public async Task<IActionResult> UploadAvatar(Guid id, IFormFile file)
    {
        var result = await _fundraisingService.UploadAvatarAsync(id, HttpContext.GetUserId(), file);
        return result.ToActionResult();
    }

    [HttpPost("{id}/delete-avatar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.User)]
    public async Task<IActionResult> DeleteAvatar(Guid id)
    {
        var result = await _fundraisingService.DeleteAvatarAsync(id, HttpContext.GetUserId());
        return result.ToActionResult();
    }
}