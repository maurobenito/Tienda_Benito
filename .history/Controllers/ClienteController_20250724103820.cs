using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Tienda_Benito.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public ClienteController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string filtro = "", string orderBy = "apellido", bool desc = false, int pagina = 1, int tamPagina = 5)
        {
            var query = _context.Cliente.AsQueryable();

            // Filtro
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(c =>
                    c.Nombre.Contains(filtro) ||
                    c.Apellido.Contains(filtro) ||
                    c.Email.Contains(filtro));
            }

            // Ordenamiento
            query = orderBy switch
            {
                "nombre" => desc ? query.OrderByDescending(c => c.Nombre) : query.OrderBy(c => c.Nombre),
                "apellido" => desc ? query.OrderByDescending(c => c.Apellido) : query.OrderBy(c => c.Apellido),
                "email" => desc ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
                _ => query.OrderBy(c => c.Apellido)
            };

            // Total y paginación
            int total = query.Count();
            var clientes = query
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

            return View(clientes);
        }

      public IActionResult Index(string filtro = "", string orderBy = "Apellido", int pagina = 1, int tamPagina = 5)
{
    var query = _context.Cliente.AsQueryable();

    // Filtro
    if (!string.IsNullOrEmpty(filtro))
    {
        query = query.Where(c =>
            c.Nombre.Contains(filtro) ||
            c.Apellido.Contains(filtro) ||
            c.Email.Contains(filtro));
    }

    // Detectar campo y dirección
    bool desc = orderBy.EndsWith("_desc");
    string campo = desc ? orderBy.Replace("_desc", "") : orderBy;

    // Ordenamiento
    query = campo switch
    {
        "Apellido" => desc ? query.OrderByDescending(c => c.Apellido) : query.OrderBy(c => c.Apellido),
        "Nombre" => desc ? query.OrderByDescending(c => c.Nombre) : query.OrderBy(c => c.Nombre),
        "Email" => desc ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
        _ => query.OrderBy(c => c.Apellido)
    };

    // Paginación
    int total = query.Count();
    var clientes = query
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    // ViewBag
    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPagina);
    ViewBag.Filtro = filtro;
    ViewBag.OrderBy = orderBy;

    return View(clientes);
}


        public IActionResult Details(int id)
        {
            var cliente = _context.Cliente.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _context.Cliente.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cliente = _context.Cliente.Find(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
