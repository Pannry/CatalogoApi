using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo._6.Controllers
{
    // https://docs.microsoft.com/pt-br/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#exception-handler

    public class ErrorController : ControllerBase
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
