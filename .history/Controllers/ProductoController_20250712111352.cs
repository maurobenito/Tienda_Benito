using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public ProductoController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Producto
                .Include(p => p.Proveedor)
                .Include(p => p.Rubro)
                .ToList();
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

        public IActionResult Create()
        {
            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre");
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre");
            ViewBag.ProductosPadre = new SelectList(
                _context.Producto.Where(p => p.ProductoPadreId == null),
                "ProductoId",
                "Nombre"
            );
            return View(new Producto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto p)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos(p);
                return View(p);
            }

            _context.Producto.Add(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto == null) return NotFound();

            CargarCombos(producto);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto p)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos(p);
                return View(p);
            }

            _context.Producto.Update(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos(Producto p)
        {
            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre", p.ProveedorId);
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre", p.RubroId);
            ViewBag.ProductosPadre = new SelectList(
                _context.Producto.Where(x => x.ProductoPadreId == null || x.ProductoId == p.ProductoPadreId),
                "ProductoId",
                "Nombre",
                p.ProductoPadreId
            );
        }

        public IActionResult Delete(int id)
        {
            var producto = _context.Producto
                .Include(p => p.Proveedor)
                .Include(p => p.Rubro)
                .FirstOrDefault(p => p.ProductoId == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                _context.SaveChanges();
            }
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
    }
}
