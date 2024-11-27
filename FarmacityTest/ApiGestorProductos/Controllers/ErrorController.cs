using Microsoft.AspNetCore.Mvc;

namespace ApiGestorProductos.Controllers
{
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError()
        {
            return Problem(detail: "Ha ocurrido un error inesperado. Por favor, inténtelo de nuevo más tarde.");
        }
    }
}
