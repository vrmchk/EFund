using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Utility;

public class NamedFormFileCollection
{
    public Dictionary<string, IFormFile> FileCollection { get; set; } = null!;
}