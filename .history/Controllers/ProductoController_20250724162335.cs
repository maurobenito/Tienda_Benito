using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace Tienda_Benito.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public ProductoController(ProyectoTiendaContext context)
        {
            _context = context;
        }

     public IActionResult Index(string nombre = "", string rubro = "", string proveedor = "", int pagina = 1, int tamPagina = 15, string orden = "")
{
    var consulta = _context.Producto
        .Include(p => p.Proveedor)
        .Include(p => p.Rubro)
        .AsQueryable();

    if (!string.IsNullOrEmpty(nombre))
        consulta = consulta.Where(p => p.Nombre.Contains(nombre));

    if (!string.IsNullOrEmpty(rubro))
        consulta = consulta.Where(p => p.Rubro.Nombre.Contains(rubro));

    if (!string.IsNullOrEmpty(proveedor))
        consulta = consulta.Where(p => p.Proveedor.Nombre.Contains(proveedor));

    // Orden dinámico
    consulta = orden switch
    {
        "nombre_desc" => consulta.OrderByDescending(p => p.Nombre),
        "precioCosto" => consulta.OrderBy(p => p.PrecioCosto),
        "precioCosto_desc" => consulta.OrderByDescending(p => p.PrecioCosto),
        "precioVenta" => consulta.OrderBy(p => p.PrecioVenta),
        "precioVenta_desc" => consulta.OrderByDescending(p => p.PrecioVenta),
        "rubro" => consulta.OrderBy(p => p.Rubro.Nombre),
        "rubro_desc" => consulta.OrderByDescending(p => p.Rubro.Nombre),
        "proveedor" => consulta.OrderBy(p => p.Proveedor.Nombre),
        "proveedor_desc" => consulta.OrderByDescending(p => p.Proveedor.Nombre),
        _ => consulta.OrderBy(p => p.Nombre),
    };

    int totalRegistros = consulta.Count();
    int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamPagina);

    var productos = consulta
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    ViewBag.NombreFiltro = nombre;
    ViewBag.RubroFiltro = rubro;
    ViewBag.ProveedorFiltro = proveedor;
    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = totalPaginas;
    ViewBag.OrdenActual = orden;

    // Combo Rubros
    ViewBag.Rubros = _context.Rubro
        .Select(r => new SelectListItem
        {
            Value = r.Nombre,
            Text = r.Nombre
        }).ToList();

    // Combo Proveedores
    ViewBag.Proveedores = _context.Proveedor
        .Select(p => new SelectListItem
        {
            Value = p.Nombre,
            Text = p.Nombre
        }).ToList();

    return View(productos);
}



        public IActionResult Details(int id)
        {
            var producto = _context.Producto
                .Include(p => p.Proveedor)
                .Include(p => p.Rubro)
                .FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CargarCombos(new Producto());
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto p)
        {
            ModelState.Remove("ProductoPadre");
            ModelState.Remove("ProductosFraccionados");

            if (!ModelState.IsValid)
            {
                CargarCombos(p);
                return View(p);
            }

            if (p.ProductoPadreId == 0)
            {
                p.ProductoPadreId = null;
                p.EquivalenciaEnPadre = null;
            }

            _context.Producto.Add(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto == null) return NotFound();

            CargarCombos(producto);
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto p)
        {
            ModelState.Remove("ProductoPadre");
            ModelState.Remove("ProductosFraccionados");

            if (!ModelState.IsValid)
            {
                CargarCombos(p);
                return View(p);
            }

            if (p.ProductoPadreId == 0)
            {
                p.ProductoPadreId = null;
                p.EquivalenciaEnPadre = null;
            }

            _context.Producto.Update(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
public IActionResult Delete(int id)
        {
            var producto = _context.Producto
                .Include(p => p.Proveedor)
                .Include(p => p.Rubro)
                .FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();
            return View(producto);
        }
        [Authorize(Roles = "Admin")]
[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

public IActionResult DeleteConfirmed(int id)
{
    var producto = _context.Producto.Find(id);

    bool usadoEnVentas = _context.Ventadetalle.Any(v => v.ProductoId == id);
    bool tieneHijos = _context.Producto.Any(p => p.ProductoPadreId == id);

    if (usadoEnVentas)
    {
        ModelState.AddModelError(string.Empty, "No se puede eliminar este producto porque ya fue usado en una venta.");
    }

    if (tieneHijos)
    {
        ModelState.AddModelError(string.Empty, "No se puede eliminar este producto porque es usado como producto fraccionado (padre).");
    }

    if (!ModelState.IsValid)
    {
        var productoCompleto = _context.Producto
            .Include(p => p.Proveedor)
            .Include(p => p.Rubro)
            .FirstOrDefault(p => p.ProductoId == id);
        return View("Delete", productoCompleto);
    }

    _context.Producto.Remove(producto);
    _context.SaveChanges();

    return RedirectToAction(nameof(Index));
}

        [HttpGet]
        public IActionResult GetPrecio(int id)
        {
            var producto = _context.Producto.FirstOrDefault(p => p.ProductoId == id);
            if (producto == null)
                return NotFound();

            return Json(new { precio = producto.PrecioVenta });
        }

        private void CargarCombos(Producto p)
        {
            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre", p.ProveedorId);
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre", p.RubroId);
            ViewBag.ProductosPadre = new SelectList(
                _context.Producto.Where(x => x.ProductoPadreId == null && x.ProductoId != p.ProductoId),
                "ProductoId",
                "Nombre",
                p.ProductoPadreId
            );
        }
    }
}
