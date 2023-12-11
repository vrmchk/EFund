using EFund.Common.Models.DTO.Error;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Extensions;

public static class LanguageExtExtensions
{
    public static IActionResult ToActionResult<T>(this Either<ErrorDTO, T> either)
    {
        return either.Match<IActionResult>(
            Left: error => new BadRequestObjectResult(error),
            Right: x => new OkObjectResult(x)
        );
    }

    public static IActionResult ToActionResult(this Option<ErrorDTO> either)
    {
        return either.Match<IActionResult>(
            Some: error => new BadRequestObjectResult(error),
            None: () => new NoContentResult()
        );
    }
}