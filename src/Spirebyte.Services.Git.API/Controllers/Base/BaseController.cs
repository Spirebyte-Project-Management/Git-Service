using Microsoft.AspNetCore.Mvc;

namespace Spirebyte.Services.Git.API.Controllers.Base;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is not null) return Ok(model);

        return NotFound();
    }
}