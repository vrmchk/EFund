using EFund.BLL.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EFund.WebAPI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpPost("file")]
    public IActionResult Test(NamedFormFile file)
    {
        return Ok(file.FileTitle != null ? "Success" : "Fail");
    }
    
    [HttpPost("files")]
    public IActionResult Test(NamedFormFileCollection files)
    {
        return Ok(files.FileCollection.Any() ? "Success" : "Fail");
    }
}