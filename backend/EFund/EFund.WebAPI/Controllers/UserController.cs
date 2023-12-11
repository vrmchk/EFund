using EFund.BLL.Services.Auth.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.User;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IValidatorService _validator;

    public UserController(IUserService userService, IPasswordService passwordService, IValidatorService validator)
    {
        _userService = userService;
        _passwordService = passwordService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetById()
    {
        var userId = HttpContext.GetUserId();
        var result = await _userService.GetByIdAsync(userId);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var userId = HttpContext.GetUserId();
        var result = await _userService.UpdateUserAsync(userId, dto);
        return result.ToActionResult();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var userId = HttpContext.GetUserId();
        var result = await _passwordService.ChangePasswordAsync(userId, dto);
        return result.ToActionResult();
    }

    [HttpPost("add-password")]
    public async Task<IActionResult> AddPassword(AddPasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var userId = HttpContext.GetUserId();
        var result = await _passwordService.AddPasswordAsync(userId, dto);
        return result.ToActionResult();
    }

    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail(ChangeEmailDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var userId = HttpContext.GetUserId();
        var result = await _userService.SendChangeEmailCodeAsync(userId, dto);
        return result.ToActionResult();
    }

    [HttpPost("confirm-change-email")]
    public async Task<IActionResult> ConfirmChangeEmail(ConfirmChangeEmailDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var userId = HttpContext.GetUserId();
        var result = await _userService.ChangeEmailAsync(userId, dto);
        return result.ToActionResult();
    }
}