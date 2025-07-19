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

        public IActionResult Index()
        {
            var clientes = _repo.ObtenerTodos();
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
            if (ModelState.IsValid)
            {
                if (cliente.ImagenSubida != null)
                {
                    string nombreImagen = Guid.NewGuid() + Path.GetExtension(cliente.ImagenSubida.FileName);
                    string rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreImagen);
                    using var stream = new FileStream(rutaImg, FileMode.Create);
                    cliente.ImagenSubida.CopyTo(stream);
                    cliente.FotoPerfil = nombreImagen;
                }

                if (cliente.ArchivoSubido != null)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(cliente.ArchivoSubido.FileName);
                    string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
                    using var stream = new FileStream(rutaArchivo, FileMode.Create);
                    cliente.ArchivoSubido.CopyTo(stream);
                    cliente.ArchivoAdjunto = nombreArchivo;
                }

                _repo.Alta(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

       

       public IActionResult Edit(int id, Cliente cliente)
{
    if (id != cliente.ClienteId)
        return NotFound();

    if (ModelState.IsValid)
    {
        var clienteExistente = _repo.ObtenerPorId(id);
        if (clienteExistente == null)
            return NotFound();

        // Actualizar campos básicos
        clienteExistente.Nombre = cliente.Nombre;
        clienteExistente.Apellido = cliente.Apellido;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Telefono = cliente.Telefono;
        clienteExistente.Direccion = cliente.Direccion;
        clienteExistente.CondFiscal = cliente.CondFiscal;
        clienteExistente.Cuit = cliente.Cuit;
        clienteExistente.UsuarioId = cliente.UsuarioId;

        // Imagen
        if (cliente.ImagenSubida != null)
        {
            string nombreImagen = Guid.NewGuid() + Path.GetExtension(cliente.ImagenSubida.FileName);
            string rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreImagen);
            using var stream = new FileStream(rutaImg, FileMode.Create);
            cliente.ImagenSubida.CopyTo(stream);
            clienteExistente.FotoPerfil = nombreImagen;
        }

        // Archivo adjunto
        if (cliente.ArchivoSubido != null)
        {
            string nombreArchivo = Guid.NewGuid() + Path.GetExtension(cliente.ArchivoSubido.FileName);
            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
            using var stream = new FileStream(rutaArchivo, FileMode.Create);
            cliente.ArchivoSubido.CopyTo(stream);
            clienteExistente.ArchivoAdjunto = nombreArchivo;
        }

        _repo.Modificacion(clienteExistente);
        return RedirectToAction(nameof(Index));
    }

    return View(cliente);
}


        public IActionResult Details(int id)
        {
            var cliente = _repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
