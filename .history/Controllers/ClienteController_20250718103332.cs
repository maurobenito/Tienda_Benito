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
            cliente.Avatar = imagenNombre;
        }

        _repo.Alta(cliente);
        return RedirectToAction(nameof(Index));
    }
    return View(cliente);
}

        public IActionResult Edit(int id)
        {
            var cliente = _repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _repo.Modificacion(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
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
