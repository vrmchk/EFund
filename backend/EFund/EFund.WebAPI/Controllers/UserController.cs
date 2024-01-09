using EFund.BLL.Services.Auth.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
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

    [HttpGet("me")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById()
    {
        var result = await _userService.GetByIdAsync(HttpContext.GetUserId(), HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpPost("search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PagedListDTO<UserDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Search([FromBody] SearchUserDTO dto, [FromQuery] PaginationDTO pagination)
    {
        var validationResult = await _validator.ValidateAsync(pagination);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.SearchAsync(dto, pagination, HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpPut("me")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.UpdateUserAsync(HttpContext.GetUserId(), dto, HttpContext.GetApiUrl());
        return result.ToActionResult();
    }

    [HttpPost("change-password")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _passwordService.ChangePasswordAsync(HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpPost("add-password")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddPassword(AddPasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _passwordService.AddPasswordAsync(HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpPost("change-email")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeEmail(ChangeEmailDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.SendChangeEmailCodeAsync(HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpPost("confirm-change-email")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConfirmChangeEmail(ConfirmChangeEmailDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.ChangeEmailAsync(HttpContext.GetUserId(), dto);
        return result.ToActionResult();
    }

    [HttpPost("upload-avatar")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var result = await _userService.UploadAvatarAsync(HttpContext.GetUserId(), file);
        return result.ToActionResult();
    }

    [HttpPost("delete-avatar")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAvatar()
    {
        var result = await _userService.DeleteAvatarAsync(HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("make-admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> MakeAdmin(MakeAdminDTO dto)
    {
        var result = await _userService.MakeAdminAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("invite-admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Invite(InviteAdminDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.InviteAdminAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("action")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> BlockUser(UserActionDTO actionDTO)
    {
        var validationResult = await _validator.ValidateAsync(actionDTO);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _userService.PerformUserActionAsync(actionDTO);
        return result.ToActionResult();
    }
}