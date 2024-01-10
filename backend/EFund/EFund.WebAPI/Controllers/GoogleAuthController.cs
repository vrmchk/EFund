using EFund.BLL.Services.Auth.Interfaces;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/google-auth")]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("sign-up")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthSuccessDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> SignUp([FromHeader(Name = "Authorization-Code")] string authorizationCode, [FromBody] GoogleSingUpDTO dto)
    {
        var result = await _googleAuthService.SignUpAsync(authorizationCode, dto);
        return result.ToActionResult();
    }

    [HttpPost("sign-in")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthSuccessDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
    public async Task<IActionResult> SignIn([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.SignInAsync(authorizationCode);
        return result.ToActionResult();
    }
}
