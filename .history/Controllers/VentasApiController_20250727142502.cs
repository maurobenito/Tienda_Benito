using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;

[ApiController]
[Route("api/ventas")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
            TipoVenta = dto.TipoVenta,
            Fecha = DateTime.Now,
            Total = 0
        };

        _context.Venta.Add(venta);
        _context.SaveChanges();

        foreach (var item in dto.Productos)
        {
            var prod = _context.Producto.FirstOrDefault(p => p.ProductoId == item.ProductoId);
            if (prod == null)
                return BadRequest($"Producto ID {item.ProductoId} no encontrado.");

            // ✅ Si es fraccionado, descuenta del padre
           if (prod.ProductoPadreId.HasValue && prod.EquivalenciaEnPadre > 0)
{
    var padre = _context.Producto.FirstOrDefault(p => p.ProductoId == prod.ProductoPadreId.Value);
    if (padre == null)
        return BadRequest($"Producto padre del producto {prod.Nombre} no encontrado.");

    var unidadesNecesarias = item.Cantidad * prod.EquivalenciaEnPadre;

    if (padre.Stock < unidadesNecesarias)
        return BadRequest($"Stock insuficiente en el producto padre ({padre.Nombre}) para vender {item.Cantidad} de {prod.Nombre}.");

    // ✅ Descuento del padre
    padre.Stock -= (decimal)unidadesNecesarias;

    // ✅ Recalcular y actualizar el stock del producto hijo basado en stock del padre
    prod.Stock = (int)(padre.Stock / prod.EquivalenciaEnPadre);
}

            else
            {
                // ✅ Producto normal (no fraccionado)
                if (prod.Stock < item.Cantidad)
                    return BadRequest($"Stock insuficiente para el producto {prod.Nombre}.");

                prod.Stock -= item.Cantidad;
            }

            // ✅ Agregar al detalle
            var detalle = new Ventadetalle
            {
                VentaId = venta.VentaId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };

            _context.Ventadetalle.Add(detalle);
            venta.Total += item.Cantidad * item.PrecioUnitario;
        }
        // Al final del foreach (justo antes del SaveChanges)
var padresModificados = dto.Productos
    .Select(p => _context.Producto.FirstOrDefault(prod => prod.ProductoId == p.ProductoId))
    .Where(p => p != null && p.ProductoPadreId == null) // solo productos padre
    .Select(p => p.ProductoId)
    .ToHashSet();

var productosFraccionados = _context.Producto
    .Where(p => p.ProductoPadreId.HasValue && padresModificados.Contains(p.ProductoPadreId.Value))
    .ToList();

foreach (var fraccionado in productosFraccionados)
{
    var padre = _context.Producto.FirstOrDefault(p => p.ProductoId == fraccionado.ProductoPadreId.Value);
    if (padre != null && fraccionado.EquivalenciaEnPadre > 0)
    {
        fraccionado.Stock = (int)(padre.Stock / fraccionado.EquivalenciaEnPadre);
    }
}

        _context.SaveChanges();
        return Ok(new { ventaId = venta.VentaId });
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.ToString());
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
            TipoVenta = venta.TipoVenta,
            Anulada = venta.Anulada,
            Detalles = detalles
        });
    }

    // DTOs
    public class VentaDto
    {
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public decimal Total { get; set; }
        public string TipoVenta { get; set; }
        public List<DetalleDto> Productos { get; set; } = new();
    }

    public class DetalleDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
