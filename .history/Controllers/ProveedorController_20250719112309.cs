using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Helpers;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public ProveedorController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber, int pageSize = 10)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["CuitSortParam"] = sortOrder == "Cuit" ? "cuit_desc" : "Cuit";

            var proveedores = from p in _context.Proveedor
                              select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                proveedores = proveedores.Where(p =>
                    p.Nombre.Contains(searchString) ||
                    p.CUIT.Contains(searchString));
            }

            proveedores = sortOrder switch
            {
                "nombre_desc" => proveedores.OrderByDescending(p => p.Nombre),
                "Cuit" => proveedores.OrderBy(p => p.CUIT),
                "cuit_desc" => proveedores.OrderByDescending(p => p.CUIT),
                _ => proveedores.OrderBy(p => p.Nombre),
            };

            int actualPage = pageNumber ?? 1;
            return View(await PaginatedList<Proveedor>.CreateAsync(proveedores.AsNoTracking(), actualPage, pageSize));
        }

        public IActionResult Details(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        public IActionResult Create() => View();

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
