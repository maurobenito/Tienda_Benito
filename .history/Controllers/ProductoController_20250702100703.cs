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

            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto p)
        {
            if (ModelState.IsValid)
            {
                _context.Producto.Add(p);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre", p.ProveedorId);
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre", p.RubroId);
            return View(p);
        }

        public IActionResult Edit(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto == null) return NotFound();

            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre", producto.ProveedorId);
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre", producto.RubroId);

            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto p)
        {
            if (ModelState.IsValid)
            {
                _context.Producto.Update(p);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Proveedores = new SelectList(_context.Proveedor, "ProveedorId", "Nombre", p.ProveedorId);
            ViewBag.Rubros = new SelectList(_context.Rubro, "RubroId", "Nombre", p.RubroId);
            return View(p);
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
    }
}
