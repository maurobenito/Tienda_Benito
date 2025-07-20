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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cliente cliente, IFormFile? FotoPerfilFile, IFormFile? ArchivoAdjuntoFile)
        {
            if (ModelState.IsValid)
            {
                // Procesar FotoPerfil
                if (FotoPerfilFile != null)
                {
                    string nombreFoto = Guid.NewGuid() + Path.GetExtension(FotoPerfilFile.FileName);
                    string rutaFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreFoto);
                    using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
                    FotoPerfilFile.CopyTo(streamFoto);
                    cliente.FotoPerfil = nombreFoto;
                }

                // Procesar ArchivoAdjunto
                if (ArchivoAdjuntoFile != null)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(ArchivoAdjuntoFile.FileName);
                    string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
                    using var streamArchivo = new FileStream(rutaArchivo, FileMode.Create);
                    ArchivoAdjuntoFile.CopyTo(streamArchivo);
                    cliente.ArchivoAdjunto = nombreArchivo;
                }

                _context.Cliente.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _context.Cliente.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente, IFormFile? FotoPerfilFile, IFormFile? ArchivoAdjuntoFile)
        {
            if (id != cliente.ClienteId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existente = _context.Cliente.Find(id);
                if (existente == null)
                    return NotFound();

                // Procesar FotoPerfil
                if (FotoPerfilFile != null)
                {
                    string nombreFoto = Guid.NewGuid() + Path.GetExtension(FotoPerfilFile.FileName);
                    string rutaFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreFoto);
                    using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
                    FotoPerfilFile.CopyTo(streamFoto);
                    existente.FotoPerfil = nombreFoto;
                }

                // Procesar ArchivoAdjunto
                if (ArchivoAdjuntoFile != null)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(ArchivoAdjuntoFile.FileName);
                    string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
                    using var streamArchivo = new FileStream(rutaArchivo, FileMode.Create);
                    ArchivoAdjuntoFile.CopyTo(streamArchivo);
                    existente.ArchivoAdjunto = nombreArchivo;
                }

                // Campos simples
                existente.Nombre = cliente.Nombre;
                existente.Apellido = cliente.Apellido;
                existente.Email = cliente.Email;
                existente.Telefono = cliente.Telefono;
                existente.Direccion = cliente.Direccion;
                existente.Cuit = cliente.Cuit;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
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
