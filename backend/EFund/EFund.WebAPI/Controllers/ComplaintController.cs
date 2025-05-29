using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.DTO.Complaint;
using EFund.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> SearchComplaints([FromQuery] SearchComplaintsDTO dto)
    {
        var complaints = await _complaintService.SearchAsync(dto);
        return Ok(complaints);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    public async Task<IActionResult> GetComplaintById(Guid id)
    {
        var result = await _complaintService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Shared)]
    public async Task<IActionResult> AddComplaint(CreateComplaintDTO dto)
    {
        var result = await _complaintService.AddAsync(HttpContext.GetUserId(), dto);
        return result.Match<IActionResult>(
            Right: complaint => CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, complaint),
            Left: BadRequest);
    }

    [HttpPost("accept")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    public async Task<IActionResult> Accept(ComplaintAcceptDTO dto)
    {
        var result = await _complaintService.AcceptAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("reject")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    public async Task<IActionResult> Reject(ComplaintRejectDTO dto)
    {
        var result = await _complaintService.RejectAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("requestChanges")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Admin)]
    public async Task<IActionResult> RequestChanges(ComplaintRequestChangesDTO dto)
    {
        var result = await _complaintService.RequestChangesAsync(dto, HttpContext.GetUserId());
        return result.ToActionResult();
    }
}