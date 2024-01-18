using Microsoft.AspNetCore.Http;

namespace EFund.Common.Models.DTO.Common;

public class NamedFormFileCollection
{
    public string? FileTitle { get; set; }
    public IFormFileCollection FileCollection { get; set; } = null!;
}