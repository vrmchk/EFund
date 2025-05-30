using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.FundraisingReview;
using EFund.Common.Models.DTO.Error;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/fundraisings/reviews")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
public class FundraisingReviewController : ControllerBase
{
    private readonly IFundraisingReviewService _fundraisingReviewService;

    public FundraisingReviewController(IFundraisingReviewService fundraisingReviewService)
    {
        _fundraisingReviewService = fundraisingReviewService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a fundraising review", Description = "Creates a new review for a fundraising.")]
    [SwaggerResponse(200, "Review created", typeof(FundraisingReviewDTO))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> AddReview([FromBody] CreateFundraisingReviewDTO dto)
    {
        var result = await _fundraisingReviewService.AddReviewAsync(HttpContext.GetUserId(), dto);
        return result.Match<IActionResult>(
            Right: review => CreatedAtAction(nameof(GetById), new { id = review.Id }, review),
            Left: BadRequest
        );
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get fundraising reviews", Description = "Returns a list of reviews filtered by fundraisingId or reviewId.")]
    [SwaggerResponse(200, "List of reviews", typeof(List<FundraisingReviewDTO>))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> GetAll([FromQuery] Guid? fundraisingId, [FromQuery] Guid? reviewId)
    {
        var result = await _fundraisingReviewService.GetAllAsync(fundraisingId, reviewId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get fundraising review by ID", Description = "Returns a fundraising review by its ID.")]
    [SwaggerResponse(200, "Review found", typeof(FundraisingReviewDTO))]
    [SwaggerResponse(400, "Review not found", typeof(ErrorDTO))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fundraisingReviewService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update fundraising review", Description = "Updates an existing fundraising review.")]
    [SwaggerResponse(200, "Review updated", typeof(FundraisingReviewDTO))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    [SwaggerResponse(400, "Review not found", typeof(ErrorDTO))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFundraisingReviewDTO dto)
    {
        var result = await _fundraisingReviewService.UpdateAsync(id, dto);
        return result.ToActionResult();
    }
}