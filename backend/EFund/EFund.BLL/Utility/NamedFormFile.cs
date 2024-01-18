using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Utility;

public class NamedFormFile
{
    public string? FileTitle { get; set; }
    
    public IFormFile File { get; set; } = null!;
}