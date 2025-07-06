using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class VentaDetalleController : Controller
    {
        private readonly IRepositorio<Ventadetalle> repo;
        private readonly ProyectoTiendaContext contexto;

        public VentaDetalleController(IRepositorio<Ventadetalle> repo, ProyectoTiendaContext contexto)
        {
            this.repo = repo;
            this.contexto = contexto;
        }

        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var entidad = repo.ObtenerPorId(id);
            if (entidad == null) return NotFound();
            return View(entidad);
        }

        public IActionResult Create()
        {
            ViewBag.Productos = new SelectList(contexto.Producto, "ProductoId", "Nombre");
            ViewBag.Ventas = new SelectList(contexto.Venta, "VentaId", "VentaId");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ventadetalle entidad)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(entidad);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Productos = new SelectList(contexto.Producto, "ProductoId", "Nombre", entidad.ProductoId);
            ViewBag.Ventas = new SelectList(contexto.Venta, "VentaId", "VentaId", entidad.VentaId);
            return View(entidad);
        }

        public IActionResult Edit(int id)
        {
            var entidad = repo.ObtenerPorId(id);
            if (entidad == null) return NotFound();

            ViewBag.Productos = new SelectList(contexto.Producto, "ProductoId", "Nombre", entidad.ProductoId);
            ViewBag.Ventas = new SelectList(contexto.Venta, "VentaId", "VentaId", entidad.VentaId);
            return View(entidad);
        }

        [HttpPost]
        public IActionResult Edit(Ventadetalle entidad)
        {
            if (ModelState.IsValid)
            {
                repo.Modificacion(entidad);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Productos = new SelectList(contexto.Producto, "ProductoId", "Nombre", entidad.ProductoId);
            ViewBag.Ventas = new SelectList(contexto.Venta, "VentaId", "VentaId", entidad.VentaId);
            return View(entidad);
        }

        public IActionResult Delete(int id)
        {
            var entidad = repo.ObtenerPorId(id);
            if (entidad == null) return NotFound();
            return View(entidad);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
