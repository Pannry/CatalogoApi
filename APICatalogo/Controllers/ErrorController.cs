using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;

namespace APICatalogo.Controllers
{
    // https://docs.microsoft.com/pt-br/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#exception-handler

    [ApiController]
    public class ErrorLocalDevelopment : ControllerBase
    {
        [HttpGet]
        [Route("/error-local-development")]
        public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        [HttpGet]
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
