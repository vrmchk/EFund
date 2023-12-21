using EFund.BLL.Services.Auth.Interfaces;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> SignUp([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.SignUpAsync(authorizationCode);
        return result.ToActionResult();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromHeader(Name = "Authorization-Code")] string authorizationCode)
    {
        var result = await _googleAuthService.SignInAsync(authorizationCode);
        return result.ToActionResult();
    }
}
