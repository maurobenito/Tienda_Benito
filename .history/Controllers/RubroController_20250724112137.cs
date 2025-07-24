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

        public IActionResult Index(string filtro = "")
{
    var consulta = _context.Rubro.AsQueryable();

    if (!string.IsNullOrEmpty(filtro))
    {
        consulta = consulta.Where(r => r.Nombre.Contains(filtro));
    }

    var lista = consulta.ToList();

    ViewBag.Filtro = filtro;

    return View(lista);
}


        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Rubro r)
        {
            if (!ModelState.IsValid) return View(r);
            _context.Rubro.Add(r);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
            if (!ModelState.IsValid) return View(r);
            _context.Rubro.Update(r);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
       [HttpGet]
public IActionResult GetRubros()
{
    var lista = _context.Rubro
        .Select(r => new { rubroId = r.RubroId, nombre = r.Nombre })
        .ToList();
    return Json(lista);
}


    }
    
}
