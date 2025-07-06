using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;


[ApiController]
[Route("api/productos")]
public class ProductosApiController : ControllerBase
{
    private readonly ProyectoTiendaContext _context;

    public ProductosApiController(ProyectoTiendaContext context)
    {
        _context = context;
    }

    [HttpGet("buscar")]
    public IActionResult Buscar(string query)
    {
        var productos = _context.Producto
            .Where(p => p.Nombre.Contains(query))
            .Select(p => new {
                p.ProductoId,
                p.Nombre,
                p.PrecioVenta,
                p.Stock
            }).ToList();

        return Ok(productos);
    }
}
