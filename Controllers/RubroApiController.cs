using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers.Api
{
    [ApiController]
    [Route("api/rubros")]
    public class RubroApiController : ControllerBase
    {
        private readonly ProyectoTiendaContext _context;

        public RubroApiController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        [HttpGet("todos")]
        public IActionResult Todos()
        {
            var rubros = _context.Rubro.Select(r => new {
                r.RubroId,
                r.Nombre
            }).ToList();

            return Ok(rubros);
        }
    }
}
