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
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Proveedor proveedor, IFormFile? Archivo1File, IFormFile? Archivo2File)
{
    if (id != proveedor.ProveedorId)
        return NotFound();

    if (ModelState.IsValid)
    {
        var existente = _context.Proveedor.Find(id);
        if (existente == null)
            return NotFound();

        // Procesar Archivo 1
        if (Archivo1File != null)
        {
            string nombreArchivo1 = Guid.NewGuid() + Path.GetExtension(Archivo1File.FileName);
            string ruta1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo1);
            using var stream1 = new FileStream(ruta1, FileMode.Create);
            Archivo1File.CopyTo(stream1);
            existente.Archivo1 = nombreArchivo1;
        }

        // Procesar Archivo 2
        if (Archivo2File != null)
        {
            string nombreArchivo2 = Guid.NewGuid() + Path.GetExtension(Archivo2File.FileName);
            string ruta2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo2);
            using var stream2 = new FileStream(ruta2, FileMode.Create);
            Archivo2File.CopyTo(stream2);
            existente.Archivo2 = nombreArchivo2;
        }

        // Campos simples
        existente.Nombre = proveedor.Nombre;
        existente.Email = proveedor.Email;
        existente.Telefono = proveedor.Telefono;
        existente.Direccion = proveedor.Direccion;
        existente.Cuit = proveedor.Cuit;
        existente.RazonSocial = proveedor.RazonSocial;
        existente.DatosBancarios = proveedor.DatosBancarios;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    return View(proveedor);
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