using Microsoft.AspNetCore.Mvc;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers
{
    public class RubroController : Controller
    {
        private readonly ProyectoTiendaContext _context;

        public RubroController(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Rubro.ToList());
        }

        public IActionResult Details(int id)
        {
            var rubro = _context.Rubro.Find(id);
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
            if (ModelState.IsValid)
            {
                _context.Rubro.Add(r);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(r);
        }

        public IActionResult Edit(int id)
        {
            var rubro = _context.Rubro.Find(id);
            if (rubro == null) return NotFound();
            return View(rubro);
        }

        [HttpPost]
        public IActionResult Edit(Rubro r)
        {
            if (ModelState.IsValid)
            {
                _context.Rubro.Update(r);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(r);
        }

        public IActionResult Delete(int id)
        {
            var rubro = _context.Rubro.Find(id);
            if (rubro == null) return NotFound();
            return View(rubro);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var rubro = _context.Rubro.Find(id);
            if (rubro != null)
            {
                _context.Rubro.Remove(rubro);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
