
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
[ApiController]
[Route("api/ventas")]
public class VentasApiController : ControllerBase
{
    private readonly ProyectoTiendaContext _context;

    public VentasApiController(ProyectoTiendaContext context)
    {
        _context = context;
    }

    [HttpPost("crear")]
    public IActionResult Crear([FromBody] VentaDto dto)
    {
        var venta = new Ventum
        {
            ClienteId = dto.ClienteId,
            UsuarioId = dto.UsuarioId,
            Fecha = DateTime.Now,
            Total = dto.Total
        };

        _context.Venta.Add(venta);
        _context.SaveChanges();

        foreach (var item in dto.Productos)
        {
            var detalle = new Ventadetalle
            {
                VentaId = venta.VentaId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };

            _context.Ventadetalle.Add(detalle);

            // Descontar stock
            var prod = _context.Producto.Find(item.ProductoId);
            if (prod != null)
            {
                prod.Stock -= item.Cantidad;
            }
        }

        _context.SaveChanges();

        return Ok(new { ventaId = venta.VentaId });
    }

    public class VentaDto
    {
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public decimal Total { get; set; }
        public List<DetalleDto> Productos { get; set; } = new();
    }

    public class DetalleDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
