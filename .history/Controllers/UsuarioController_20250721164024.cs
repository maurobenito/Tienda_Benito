using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Para IFormFile
using Tienda_Benito.Models;
using System.IO;

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

            // Filtro
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(u =>
                    u.Nombre.Contains(filtro) ||
                    u.Apellido.Contains(filtro) ||
                    u.Email.Contains(filtro) ||
                    u.Rol.Contains(filtro));
            }

            // Ordenamiento
            query = orderBy switch
            {
                "Nombre" => desc ? query.OrderByDescending(u => u.Nombre) : query.OrderBy(u => u.Nombre),
                "Apellido" => desc ? query.OrderByDescending(u => u.Apellido) : query.OrderBy(u => u.Apellido),
                "Email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "Rol" => desc ? query.OrderByDescending(u => u.Rol) : query.OrderBy(u => u.Rol),
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
[ValidateAntiForgeryToken]
public IActionResult Create(Usuario usuario, IFormFile? AvatarFile, string? Password)
{
    if (ModelState.IsValid)
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError("Password", "La contraseña es obligatoria");
            return View(usuario);
        }

        // Hashear la contraseña con BCrypt
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);

        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            AvatarFile.CopyTo(stream);
            usuario.Avatar = fileName;
        }

        _context.Usuario.Add(usuario);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    return View(usuario);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Usuario usuario, IFormFile? FotoPerfilFile)
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

        // Actualizar campos
        existente.Email = usuario.Email;
        existente.PasswordHash = usuario.PasswordHash; // Podés cifrar si querés
        existente.Rol = usuario.Rol;
        existente.Nombre = usuario.Nombre;
        existente.Apellido = usuario.Apellido;

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
        [ValidateAntiForgeryToken]
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
