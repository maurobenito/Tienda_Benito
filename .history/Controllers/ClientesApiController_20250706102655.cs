using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesApiController : ControllerBase
    {
        private readonly ProyectoTiendaContext _context;

        public ClientesApiController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        [HttpGet("buscar")]
        public IActionResult Buscar(string query)
        {
            var clientes = _context.Cliente
                .Where(c => c.Nombre.Contains(query) || c.Apellido.Contains(query))
                .Select(c => new
                {
                    c.ClienteId,
                    nombreCompleto = c.Nombre + " " + c.Apellido
                })
                .ToList();

            return Ok(clientes);
        }
    }
}
