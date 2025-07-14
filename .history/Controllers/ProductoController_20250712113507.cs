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
            CargarCombos(new Producto());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto p)
        {
            // Evitamos validaciones innecesarias
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
            // Evitamos validaciones innecesarias
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
