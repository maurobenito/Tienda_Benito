using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using BCrypt.Net;

namespace Tienda_Benito.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public UsuarioController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string filtro = "", string orderBy = "Nombre", bool desc = false, int pagina = 1, int tamPagina = 5)
        {
            var query = _context.Usuario.AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(u =>
                    u.Nombre.Contains(filtro) ||
                    u.Apellido.Contains(filtro) ||
                    u.Email.Contains(filtro));
            }

            query = orderBy switch
            {
                "Nombre" => desc ? query.OrderByDescending(u => u.Nombre) : query.OrderBy(u => u.Nombre),
                "Apellido" => desc ? query.OrderByDescending(u => u.Apellido) : query.OrderBy(u => u.Apellido),
                "Email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                _ => query.OrderBy(u => u.Nombre)
            };

            int total = query.Count();
            var usuarios = query
                .Skip((pagina - 1) * tamPagina)
                .Take(tamPagina)
                .ToList();

            ViewBag.PaginaActual = pagina;
            ViewBag.TamanoPagina = tamPagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPagina);
            ViewBag.Filtro = filtro;
            ViewBag.OrderBy = orderBy;
            ViewBag.Desc = desc;

            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario, string Password, IFormFile? FotoPerfilFile)
        {
            if (ModelState.IsValid)
            {
                if (FotoPerfilFile != null)
                {
                    string nombreFoto = Guid.NewGuid() + Path.GetExtension(FotoPerfilFile.FileName);
                    string rutaFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreFoto);
                    using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
                    FotoPerfilFile.CopyTo(streamFoto);
                    usuario.Avatar = nombreFoto;
                }

                if (!string.IsNullOrWhiteSpace(Password))
                {
                    usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
                }

                _context.Usuario.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Usuario usuario, string? Password, IFormFile? FotoPerfilFile)
        {
            if (id != usuario.UsuarioId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existente = _context.Usuario.Find(id);
                if (existente == null)
                    return NotFound();

                if (FotoPerfilFile != null)
                {
                    string nombreFoto = Guid.NewGuid() + Path.GetExtension(FotoPerfilFile.FileName);
                    string rutaFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreFoto);
                    using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
                    FotoPerfilFile.CopyTo(streamFoto);
                    existente.Avatar = nombreFoto;
                }

                if (!string.IsNullOrWhiteSpace(Password))
                {
                    existente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
                }

                existente.Email = usuario.Email;
                existente.Nombre = usuario.Nombre;
                existente.Apellido = usuario.Apellido;
                existente.Rol = usuario.Rol;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        public IActionResult Details(int id)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
