using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IRepositorioProveedor _repo;

        public ProveedorController(IRepositorioProveedor repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var proveedor = _repo.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _repo.Agregar(proveedor);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public IActionResult Edit(int id)
        {
            var proveedor = _repo.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _repo.Actualizar(proveedor);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public IActionResult Delete(int id)
        {
            var proveedor = _repo.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
