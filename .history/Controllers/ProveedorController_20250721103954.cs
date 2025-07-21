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
public IActionResult Index(string filtro = "", string orderBy = "Nombre", bool desc = false, int pagina = 1, int tamPagina = 5)
{
    var query = _context.Proveedor.AsQueryable();

    // Filtro
    if (!string.IsNullOrEmpty(filtro))
    {
        query = query.Where(p =>
            p.Nombre.Contains(filtro) ||
            p.RazonSocial.Contains(filtro) ||
            p.Email.Contains(filtro));
    }

    // Ordenamiento
    query = orderBy switch
    {
        "Nombre" => desc ? query.OrderByDescending(p => p.Nombre) : query.OrderBy(p => p.Nombre),
        "RazonSocial" => desc ? query.OrderByDescending(p => p.RazonSocial) : query.OrderBy(p => p.RazonSocial),
        "Email" => desc ? query.OrderByDescending(p => p.Email) : query.OrderBy(p => p.Email),
        _ => query.OrderBy(p => p.Nombre)
    };

    // Total y paginación
    int total = query.Count();
    var proveedores = query
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    // Datos de paginación y filtro para la vista
    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPagina);
    ViewBag.Filtro = filtro;
    ViewBag.OrderBy = orderBy;
    ViewBag.Desc = desc;

    return View(proveedores);
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
public IActionResult Details(int id)
{
    var proveedor = _context.Proveedor.Find(id);
    if (proveedor == null) return NotFound();
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