using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public ProveedorController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Proveedor.ToList());
        }

        public IActionResult Details(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Proveedor p)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedor.Add(p);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        public IActionResult Edit(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult Edit(Proveedor p)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedor.Update(p);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        public IActionResult Delete(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
