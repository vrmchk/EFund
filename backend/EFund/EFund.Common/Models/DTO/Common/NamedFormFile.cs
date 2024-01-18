using Microsoft.AspNetCore.Http;

namespace EFund.Common.Models.DTO.Common;

public class NamedFormFile
{
    public string? FileTitle { get; set; }
    
    public IFormFile File { get; set; } = null!;
}