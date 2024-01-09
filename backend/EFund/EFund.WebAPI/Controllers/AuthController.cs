using EFund.BLL.Services.Auth.Interfaces;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IPasswordService _passwordService;
    private readonly IValidatorService _validator;

    public AuthController(IAuthService authService,
        IEmailConfirmationService emailConfirmationService,
        IRefreshTokenService refreshTokenService,
        IPasswordService passwordService,
        IValidatorService validator)
    {
        _authService = authService;
        _emailConfirmationService = emailConfirmationService;
        _refreshTokenService = refreshTokenService;
        _passwordService = passwordService;
        _validator = validator;
    }

    [HttpPost("sign-up")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SignUpResponseDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> SignUp(SignUpDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _authService.SignUpAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("confirm-email")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthSuccessDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _emailConfirmationService.ConfirmEmailAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("sign-in")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthSuccessDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> SignIn(SignInDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _authService.SignInAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("refresh-token")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthSuccessDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> RefreshToken(RefreshTokenDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _refreshTokenService.RefreshTokenAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("resend-confirmation-code")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> ResendConfirmationCode(ResendConfirmationCodeDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _emailConfirmationService.ResendConfirmationCodeAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("forgot-password")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _passwordService.ForgotPasswordAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("reset-password")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _passwordService.ResetPasswordAsync(dto);
        return result.ToActionResult();
    }
}