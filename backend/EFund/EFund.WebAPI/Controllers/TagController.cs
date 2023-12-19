using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO;
using EFund.Common.Models.DTO.Tag;
using EFund.Validation;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly IValidatorService _validator;

    public TagController(ITagService tagService, IValidatorService validator)
    {
        _tagService = tagService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationDTO pagination)
    {
        var validationResult = await _validator.ValidateAsync(pagination);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _tagService.GetAllAsync(pagination);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    public async Task<IActionResult> Add([FromBody] CreateTagDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _tagService.AddAsync(dto);
        return result.Match<IActionResult>(
            Right: tag => CreatedAtAction(nameof(GetByName), new { name = tag.Name }, tag),
            Left: BadRequest
        );
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name, [FromQuery] PaginationDTO pagination)
    {
        var validationResult = await _validator.ValidateAsync(pagination);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToErrorDTO());

        var result = await _tagService.GetByNameAsync(name, pagination);
        return Ok(result);
    }
    
    [HttpDelete("{name}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    public async Task<IActionResult> Delete(string name)
    {
        var result = await _tagService.DeleteAsync(name);
        return result.ToActionResult();
    }
}