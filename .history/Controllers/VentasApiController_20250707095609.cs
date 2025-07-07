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

    // ✅ POST para crear una nueva venta
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

    // ✅ GET para obtener detalles de una venta por ID (usado por Vue)
    [HttpGet("{id}")]
    public IActionResult ObtenerDetalleVenta(int id)
    {
        var venta = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .FirstOrDefault(v => v.VentaId == id);

        if (venta == null)
            return NotFound();

        var detalles = _context.Ventadetalle
            .Include(d => d.Producto)
            .Where(d => d.VentaId == id)
            .Select(d => new
            {
                d.Producto.Nombre,
                d.Cantidad,
                d.PrecioUnitario,
                Subtotal = d.Cantidad * d.PrecioUnitario
            }).ToList();

        return Ok(new
        {
            venta.VentaId,
            Cliente = venta.Cliente.Nombre + " " + venta.Cliente.Apellido,
            Usuario = venta.Usuario.Email, // Cambiado a Email porque 'Nombre' no existe en Usuario
            Fecha = venta.Fecha.ToString("yyyy-MM-dd HH:mm"),
            venta.Total,
            Detalles = detalles
        });
    }

    // DTOs
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
