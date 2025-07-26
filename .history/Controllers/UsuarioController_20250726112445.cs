using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Para IFormFile
using Tienda_Benito.Models;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authorization; // <- Agrega este using


namespace Tienda_Benito.Controllers
{

    public class UsuarioController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public UsuarioController(ProyectoTiendaContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // Claims para identidad
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre + " " + usuario.Apellido),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioId", usuario.UsuarioId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    [Authorize(Roles = "Admin")]
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

    // Detectar orden descendente por sufijo "_desc"
    bool descending = orderBy.EndsWith("_desc");
    string campoOrden = descending ? orderBy.Replace("_desc", "") : orderBy;

    // Ordenamiento dinámico
    query = campoOrden switch
    {
        "Nombre" => descending ? query.OrderByDescending(u => u.Nombre) : query.OrderBy(u => u.Nombre),
        "Apellido" => descending ? query.OrderByDescending(u => u.Apellido) : query.OrderBy(u => u.Apellido),
        "Email" => descending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
        "Rol" => descending ? query.OrderByDescending(u => u.Rol) : query.OrderBy(u => u.Rol),
        _ => query.OrderBy(u => u.Nombre)
    };

    // Paginación
    int total = query.Count();
    var usuarios = query.Skip((pagina - 1) * tamPagina).Take(tamPagina).ToList();

    // Pasar valores a la vista
    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPagina);
    ViewBag.Filtro = filtro;
    ViewBag.OrderBy = orderBy;
    ViewBag.Desc = descending;

    return View(usuarios);
}



    [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario, IFormFile? AvatarFile, string? Password)
        {
            // Eliminar el error de ModelState sobre PasswordHash para que no bloquee
            ModelState.Remove("PasswordHash");

            if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "La contraseña es obligatoria");
            }

            if (ModelState.IsValid)
            {
                // Asignar el hash de la contraseña antes de guardar
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password); 

                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                    string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
                    using var stream = new FileStream(ruta, FileMode.Create);
                    AvatarFile.CopyTo(stream);
                    usuario.Avatar = nombreArchivo;
                }

                _context.Usuario.Add(usuario);
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar usuario: " + ex.Message);
                }
            }
            return View(usuario);
        }
            [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.UsuarioId == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }


[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Usuario usuario, IFormFile? FotoPerfilFile, string? Password)
{
    if (id != usuario.UsuarioId)
        return NotFound();

    // 💡 Eliminamos este campo del ModelState para que no exija el valor
    ModelState.Remove("PasswordHash");

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

        // ✅ Solo actualizar si se ingresó un nuevo password
        if (!string.IsNullOrWhiteSpace(Password))
        {
            existente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        existente.Email = usuario.Email;
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
    [Authorize(Roles = "Admin")]
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
        //         [HttpGet]
        // public IActionResult FixPasswords()
        // {
        //     var usuariosPorArreglar = _context.Usuario
        //         .Where(u => u.PasswordHash.Length < 20) // Suponemos que los no hasheados son cortos
        //         .ToList();

        //     int actualizados = 0;

        //     foreach (var u in usuariosPorArreglar)
        //     {
        //         u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);
        //         actualizados++;
        //     }

        //     _context.SaveChanges();

        //     return Content($"Contraseñas corregidas: {actualizados}");
        // }
[HttpGet]
public IActionResult AccessDenied()
{
    return View();
}

    }
}
