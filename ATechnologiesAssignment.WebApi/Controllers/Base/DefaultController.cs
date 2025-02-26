using ATechnologiesAssignment.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace ATechnologiesAssignment.WebApi.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class DefaultController : ControllerBase
    {
        protected virtual IActionResult HandleResponse(BaseResponse response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
