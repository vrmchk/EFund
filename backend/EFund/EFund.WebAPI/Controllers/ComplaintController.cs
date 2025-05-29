using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Complaint;
using EFund.Common.Models.DTO.Error;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/complaints")]
public class ComplaintController : ControllerBase
{
    private readonly IComplaintService _complaintService;

    public ComplaintController(IComplaintService complaintService)
    {
        _complaintService = complaintService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerOperation(Summary = "Search complaints", Description = "Returns a list of complaints filtered by query parameters.")]
    [SwaggerResponse(200, "List of complaints", typeof(List<ComplaintDTO>))]
    public async Task<IActionResult> SearchComplaints([FromQuery] SearchComplaintsDTO dto)
    {
        var complaints = await _complaintService.SearchAsync(dto);
        return Ok(complaints);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    [SwaggerOperation(Summary = "Get complaint by Id", Description = "Returns a complaint by its Id.")]
    [SwaggerResponse(200, "Complaint found", typeof(ComplaintDTO))]
    [SwaggerResponse(404, "Complaint not found", typeof(ErrorDTO))]
    public async Task<IActionResult> GetComplaintById(Guid id)
    {
        var result = await _complaintService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    [SwaggerOperation(Summary = "Create a complaint", Description = "Creates a new complaint for a fundraising.")]
    [SwaggerResponse(201, "Complaint created", typeof(ComplaintDTO))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> AddComplaint(CreateComplaintDTO dto)
    {
        var result = await _complaintService.AddAsync(HttpContext.GetUserId(), dto);
        return result.Match<IActionResult>(
            Right: complaint => CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, complaint),
            Left: BadRequest);
    }

    [HttpPost("accept")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerOperation(Summary = "Accept a complaint", Description = "Accepts a complaint and triggers post-accept actions.")]
    [SwaggerResponse(204, "Complaint accepted")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> Accept(ComplaintAcceptDTO dto)
    {
        var result = await _complaintService.AcceptAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("reject")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerOperation(Summary = "Reject a complaint", Description = "Rejects a complaint.")]
    [SwaggerResponse(204, "Complaint rejected")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> Reject(ComplaintRejectDTO dto)
    {
        var result = await _complaintService.RejectAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("requestChanges")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    [SwaggerOperation(Summary = "Request changes for a complaint", Description = "Requests changes for a complaint and triggers post-request actions.")]
    [SwaggerResponse(204, "Changes requested")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorDTO))]
    public async Task<IActionResult> RequestChanges(ComplaintRequestChangesDTO dto)
    {
        var result = await _complaintService.RequestChangesAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }
}