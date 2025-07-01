using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class VentaController : Controller
    {
        private readonly IRepositorio<Ventum> _repositorio;
        private readonly ProyectoTiendaContext _context;

        public VentaController(IRepositorio<Ventum> repositorio, ProyectoTiendaContext context)
        {
            _repositorio = repositorio;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_repositorio.ObtenerTodos());
        }

        public IActionResult Details(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);
            if (venta == null) return NotFound();
            return View(venta);
        }

        public IActionResult Create()
        {
            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Nombre");
            ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ventum v)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Alta(v);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Nombre", v.UsuarioId);
            ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre", v.ClienteId);
            return View(v);
        }

        public IActionResult Edit(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);
            if (venta == null) return NotFound();
            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Nombre", venta.UsuarioId);
            ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre", venta.ClienteId);
            return View(venta);
        }

        [HttpPost]
        public IActionResult Edit(Ventum v)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Modificacion(v);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Nombre", v.UsuarioId);
            ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre", v.ClienteId);
            return View(v);
        }

        public IActionResult Delete(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);
            if (venta == null) return NotFound();
            return View(venta);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorio.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
