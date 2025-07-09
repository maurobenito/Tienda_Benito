using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using System.Linq;

namespace Tienda_Benito.Controllers.Api
{
    [Route("api/productos")]
    [ApiController]
    public class ApiProductosController : ControllerBase
    {
        private readonly ProyectoTiendaContext _context;

        public ApiProductosController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        [HttpGet("buscar")]
        public IActionResult Buscar(string query)
        {
            var resultados = _context.Producto
                .Where(p => p.Nombre.Contains(query))
                .Select(p => new
                {
                    productoId = p.ProductoId,
                    nombre = p.Nombre,
                    precioVenta = p.PrecioVenta,
                    stock = p.Stock
                })
                .Take(10)
                .ToList();

            return Ok(resultados);
        }
        [HttpGet("porrubro/{rubroId}")]
public IActionResult PorRubro(int rubroId)
{
    var productos = _context.Producto
        .Where(p => p.RubroId == rubroId)
        .Select(p => new {
            productoId = p.ProductoId,
            nombre = p.Nombre,
            precioVenta = p.PrecioVenta,
            stock = p.Stock
        })
        .ToList();

    return Ok(productos);
}
[HttpGet("todos")]
public IActionResult ObtenerTodos()
{
    var productos = _context.Producto
        .Select(p => new
        {
            productoId = p.ProductoId,
            nombre = p.Nombre,
            precioVenta = p.PrecioVenta,
            stock = p.Stock
        })
        .ToList();

    return Ok(productos);
}

    }
}
