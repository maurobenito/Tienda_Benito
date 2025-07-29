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


        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Cliente cliente)
{
    if (!ModelState.IsValid)
        return View(cliente);

    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    Directory.CreateDirectory(uploads);

    // Foto de perfil
    if (cliente.ImagenSubida != null && cliente.ImagenSubida.Length > 0)
    {
        string nombreFoto = Guid.NewGuid() + Path.GetExtension(cliente.ImagenSubida.FileName);
        string rutaFoto = Path.Combine(uploads, nombreFoto);
        using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
        cliente.ImagenSubida.CopyTo(streamFoto);
        cliente.FotoPerfil = nombreFoto; // <-- se guarda el nombre en BD
    }

    // Archivo adjunto
    if (cliente.ArchivoSubido != null && cliente.ArchivoSubido.Length > 0)
    {
        string nombreArchivo = Guid.NewGuid() + Path.GetExtension(cliente.ArchivoSubido.FileName);
        string rutaArchivo = Path.Combine(uploads, nombreArchivo);
        using var streamArchivo = new FileStream(rutaArchivo, FileMode.Create);
        cliente.ArchivoSubido.CopyTo(streamArchivo);
        cliente.ArchivoAdjunto = nombreArchivo; // <-- se guarda el nombre en BD
    }

    _context.Cliente.Add(cliente);
    _context.SaveChanges();
    return RedirectToAction(nameof(Index));
}

     public IActionResult Edit(int id)
{
    var cliente = _context.Cliente.Find(id);
    if (cliente == null) return NotFound();
    return View(cliente);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Cliente cliente)
{
    if (id != cliente.ClienteId)
        return NotFound();

    if (!ModelState.IsValid)
        return View(cliente);

    var existente = _context.Cliente.Find(id);
    if (existente == null)
        return NotFound();

    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    Directory.CreateDirectory(uploads);

    // Procesar FotoPerfil (ImagenSubida)
    if (cliente.ImagenSubida != null && cliente.ImagenSubida.Length > 0)
    {
        string nombreFoto = Guid.NewGuid() + Path.GetExtension(cliente.ImagenSubida.FileName);
        string rutaFoto = Path.Combine(uploads, nombreFoto);
        using var streamFoto = new FileStream(rutaFoto, FileMode.Create);
        cliente.ImagenSubida.CopyTo(streamFoto);

        // Borrar archivo anterior si existe
        if (!string.IsNullOrEmpty(existente.FotoPerfil))
        {
            var rutaAnterior = Path.Combine(uploads, existente.FotoPerfil);
            if (System.IO.File.Exists(rutaAnterior))
                System.IO.File.Delete(rutaAnterior);
        }

        existente.FotoPerfil = nombreFoto;
    }

    // Procesar ArchivoAdjunto (ArchivoSubido)
    if (cliente.ArchivoSubido != null && cliente.ArchivoSubido.Length > 0)
    {
        string nombreArchivo = Guid.NewGuid() + Path.GetExtension(cliente.ArchivoSubido.FileName);
        string rutaArchivo = Path.Combine(uploads, nombreArchivo);
        using var streamArchivo = new FileStream(rutaArchivo, FileMode.Create);
        cliente.ArchivoSubido.CopyTo(streamArchivo);

        // Borrar archivo anterior si existe
        if (!string.IsNullOrEmpty(existente.ArchivoAdjunto))
        {
            var rutaAnterior = Path.Combine(uploads, existente.ArchivoAdjunto);
            if (System.IO.File.Exists(rutaAnterior))
                System.IO.File.Delete(rutaAnterior);
        }

        existente.ArchivoAdjunto = nombreArchivo;
    }

    // Actualizar campos normales
    existente.Nombre = cliente.Nombre;
    existente.Apellido = cliente.Apellido;
    existente.Email = cliente.Email;
    existente.Telefono = cliente.Telefono;
    existente.Direccion = cliente.Direccion;
    existente.CondFiscal = cliente.CondFiscal;
    existente.Cuit = cliente.Cuit;
    // Agregá más campos si tu entidad los tiene

    _context.Update(existente);
    _context.SaveChanges();

    return RedirectToAction(nameof(Index));
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
