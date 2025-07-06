using Microsoft.AspNetCore.Mvc;
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
    try
    {
        if (dto == null || dto.Productos == null || dto.Productos.Count == 0)
            return BadRequest("Datos de la venta incompletos.");

        var venta = new Ventum
        {
            ClienteId = dto.ClienteId,
            UsuarioId = dto.UsuarioId,
            Fecha = DateTime.Now,
            Total = 0
        };

        _context.Venta.Add(venta);
        _context.SaveChanges();

        foreach (var item in dto.Productos)
        {
            var prod = _context.Producto.Find(item.ProductoId);
            if (prod == null)
                return BadRequest($"Producto ID {item.ProductoId} no encontrado.");

            if (prod.Stock < item.Cantidad)
                return BadRequest($"Stock insuficiente para el producto {prod.Nombre}.");

            var detalle = new Ventadetalle
            {
                VentaId = venta.VentaId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };

            _context.Ventadetalle.Add(detalle);
            prod.Stock -= item.Cantidad;
            venta.Total += item.Cantidad * item.PrecioUnitario;
        }

        _context.SaveChanges();

        return Ok(new { ventaId = venta.VentaId });
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.ToString()); // Devuelve el stacktrace completo
    }
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
