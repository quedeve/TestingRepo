using application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        public IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result);
        }
    }
}
