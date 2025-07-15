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

        // Método para paginación y filtros combinados
        [HttpGet]
        public IActionResult ObtenerPaginado(
            string query = "",
            int page = 1,
            int pageSize = 10,
            int? rubroId = null,
            int? proveedorId = null)
        {
            var productosQuery = _context.Producto.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                productosQuery = productosQuery.Where(p => p.Nombre.Contains(query));

            if (rubroId.HasValue && rubroId.Value > 0)
                productosQuery = productosQuery.Where(p => p.RubroId == rubroId.Value);

            if (proveedorId.HasValue && proveedorId.Value > 0)
                productosQuery = productosQuery.Where(p => p.ProveedorId == proveedorId.Value);

            var total = productosQuery.Count();

            var productos = productosQuery
                .OrderBy(p => p.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    productoId = p.ProductoId,
                    nombre = p.Nombre,
                    precioVenta = p.PrecioVenta,
                    stock = p.Stock,
                    unidadMedida = p.UnidadMedida,
                    productoPadreId = p.ProductoPadreId,
                    equivalenciaEnPadre = p.EquivalenciaEnPadre
                })
                .ToList();

            return Ok(new
            {
                total,
                items = productos
            });
        }

        // Para compatibilidad con búsqueda simple anterior, opcional
        [HttpGet("buscar")]
        public IActionResult Buscar(string query = "")
        {
            var resultados = _context.Producto
                .Where(p => p.Nombre.Contains(query ?? ""))
                .Select(p => new
                {
                    productoId = p.ProductoId,
                    nombre = p.Nombre,
                    precioVenta = p.PrecioVenta,
                    stock = p.Stock,
                    unidadMedida = p.UnidadMedida,
                    productoPadreId = p.ProductoPadreId,
                    equivalenciaEnPadre = p.EquivalenciaEnPadre
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
                .Select(p => new
                {
                    productoId = p.ProductoId,
                    nombre = p.Nombre,
                    precioVenta = p.PrecioVenta,
                    stock = p.Stock,
                    unidadMedida = p.UnidadMedida,
                    productoPadreId = p.ProductoPadreId,
                    equivalenciaEnPadre = p.EquivalenciaEnPadre
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
                    stock = p.Stock,
                    unidadMedida = p.UnidadMedida,
                    productoPadreId = p.ProductoPadreId,
                    equivalenciaEnPadre = p.EquivalenciaEnPadre
                })
                .ToList();

            return Ok(productos);
        }
        [HttpGet("filtrar")]
public IActionResult Filtrar(string? query = "", int? rubroId = null, int? proveedorId = null, int page = 1, int pageSize = 10)
{
    var productos = _context.Producto
        .Where(p =>
            (string.IsNullOrEmpty(query) || p.Nombre.Contains(query)) &&
            (!rubroId.HasValue || p.RubroId == rubroId.Value) &&
            (!proveedorId.HasValue || p.ProveedorId == proveedorId.Value))
        .Select(p => new
        {
            p.ProductoId,
            p.Nombre,
            p.PrecioVenta,
            p.Stock,
            p.UnidadMedida,
            p.ProductoPadreId,
            p.EquivalenciaEnPadre,
            Rubro = p.Rubro.Nombre,
            Proveedor = p.Proveedor.Nombre
        });

    var total = productos.Count();

    var resultados = productos
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    return Ok(new
    {
        Total = total,
        Data = resultados
    });
}

    }
}
