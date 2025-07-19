using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IRepositorio<Cliente> _repo;

        public ClienteController(IRepositorio<Cliente> repo)
        {
            _repo = repo;
        }

        public IActionResult Index() => View(_repo.ObtenerTodos());

        public IActionResult Details(int id)
        {
            var cliente = _repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

      [HttpPost]
public IActionResult Create(Cliente cliente, IFormFile? archivoSubido, IFormFile? imagenSubida)
{
    if (ModelState.IsValid)
    {
        if (archivoSubido != null)
        {
            string archivoNombre = Guid.NewGuid() + Path.GetExtension(archivoSubido.FileName);
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", archivoNombre);
            using var stream = new FileStream(ruta, FileMode.Create);
            archivoSubido.CopyTo(stream);
            cliente.ArchivoAdjunto = archivoNombre;
        }

        if (imagenSubida != null)
        {
            string imagenNombre = Guid.NewGuid() + Path.GetExtension(imagenSubida.FileName);
            string rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", imagenNombre);
            using var streamImg = new FileStream(rutaImg, FileMode.Create);
            imagenSubida.CopyTo(streamImg);
            cliente.FotoPerfil = imagenNombre;
        }

        _repo.Alta(cliente);
        return RedirectToAction(nameof(Index));
    }
    return View(cliente);
}

        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Cliente cliente)
{
    if (id != cliente.ClienteId)
        return NotFound();

    if (ModelState.IsValid)
    {
        // Actualizar ArchivoAdjunto si se sube uno nuevo
        if (cliente.ArchivoSubido != null)
        {
            string nombreArchivo = Guid.NewGuid() + Path.GetExtension(cliente.ArchivoSubido.FileName);
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
            using var stream = new FileStream(ruta, FileMode.Create);
            cliente.ArchivoSubido.CopyTo(stream);
            cliente.ArchivoAdjunto = nombreArchivo;
        }

        // Actualizar FotoPerfil si se sube una nueva
        if (cliente.ImagenSubida != null)
        {
            string nombreImagen = Guid.NewGuid() + Path.GetExtension(cliente.ImagenSubida.FileName);
            string rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreImagen);
            using var stream = new FileStream(rutaImg, FileMode.Create);
            cliente.ImagenSubida.CopyTo(stream);
            cliente.FotoPerfil = nombreImagen;
        }

                _repo.Modificacion(cliente);
                return RedirectToAction(nameof(Index));
    }

                _repo.Modificacion(cliente);
                return RedirectToAction(nameof(Index));
}

public IActionResult Delete(int id)
{
    var cliente = _repo.ObtenerPorId(id);
    if (cliente == null) return NotFound();
    return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
