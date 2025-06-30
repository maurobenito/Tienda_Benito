using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class RubroController : Controller
    {
        private readonly IRepositorio<Rubro> _repositorio;

        public RubroController(IRepositorio<Rubro> repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Index()
        {
            var lista = _repositorio.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var rubro = _repositorio.ObtenerPorId(id);
            if (rubro == null) return NotFound();
            return View(rubro);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Rubro r)
        {
            if (!ModelState.IsValid) return View(r);
            _repositorio.Alta(r);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var rubro = _repositorio.ObtenerPorId(id);
            if (rubro == null) return NotFound();
            return View(rubro);
        }

        [HttpPost]
        public IActionResult Edit(Rubro r)
        {
            if (!ModelState.IsValid) return View(r);
            _repositorio.Modificacion(r);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var rubro = _repositorio.ObtenerPorId(id);
            if (rubro == null) return NotFound();
            return View(rubro);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorio.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
