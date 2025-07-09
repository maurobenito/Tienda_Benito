using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using System.Linq;

namespace Tienda_Benito.Controllers.Api
{
    [Route("api/clientes")]
    [ApiController]
    public class ApiClientesController : ControllerBase
    {
        private readonly ProyectoTiendaContext _context;

        public ApiClientesController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        [HttpGet("buscar")]
        public IActionResult Buscar(string query)
        {
            var resultados = _context.Cliente
                .Where(c => c.Nombre.Contains(query) || c.Apellido.Contains(query))
                .Select(c => new
                {
                    clienteId = c.ClienteId,
                    nombreCompleto = c.Nombre + " " + c.Apellido
                })
                .Take(10)
                .ToList();

            return Ok(resultados);
        }
        [HttpGet("todos")]
public IActionResult ObtenerTodos()
{
    var clientes = _context.Cliente
        .Select(c => new
        {
            clienteId = c.ClienteId,
            nombreCompleto = c.Nombre + " " + c.Apellido
        })
        .ToList();

    return Ok(clientes);
}

    }
}
