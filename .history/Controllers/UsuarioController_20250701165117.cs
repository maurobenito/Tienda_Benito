// UsuarioController.cs
using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IRepositorio<Usuario> repositorio;

        public UsuarioController(IRepositorio<Usuario> repositorio)
        {
            this.repositorio = repositorio;
        }

        public IActionResult Index()
        {
            return View(repositorio.ObtenerTodos());
        }

        public IActionResult Details(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario u)
        {
            if (ModelState.IsValid)
            {
                repositorio.Alta(u);
                return RedirectToAction(nameof(Index));
            }
            return View(u);
        }

        public IActionResult Edit(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario u)
        {
            if (ModelState.IsValid)
            {
                repositorio.Modificacion(u);
                return RedirectToAction(nameof(Index));
            }
            return View(u);
        }

        public IActionResult Delete(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            repositorio.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
