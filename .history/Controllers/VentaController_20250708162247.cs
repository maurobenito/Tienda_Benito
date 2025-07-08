using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

      public IActionResult Index(int pagina = 1, int tamPagina = 5)
{
    var ventas = _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .OrderByDescending(v => v.Fecha);

    int totalVentas = ventas.Count();
    var ventasPaginadas = ventas
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalVentas / tamPagina);

    return View(ventasPaginadas);
}
        public IActionResult Details(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);
            if (venta == null) return NotFound();
            return View(venta);
        }

        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre");
            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Email");
            ViewBag.Productos = new SelectList(_context.Producto, "ProductoId", "Nombre");

            var model = new VentaViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Ventum venta, int[] productoIds, int[] cantidades, decimal[] preciosUnitarios)
        {
            if (productoIds.Length != cantidades.Length || cantidades.Length != preciosUnitarios.Length)
            {
                ModelState.AddModelError("", "Datos de productos incompletos");
                return View(venta);
            }

            venta.Fecha = DateTime.Now;
            venta.Total = 0;
            _context.Venta.Add(venta);
            _context.SaveChanges();

            for (int i = 0; i < productoIds.Length; i++)
            {
                var producto = _context.Producto.FirstOrDefault(p => p.ProductoId == productoIds[i]);
                if (producto == null || producto.Stock < cantidades[i])
                {
                    ModelState.AddModelError("", $"Stock insuficiente para el producto ID {productoIds[i]}");
                    return View(venta);
                }

                var detalle = new Ventadetalle
                {
                    VentaId = venta.VentaId,
                    ProductoId = productoIds[i],
                    Cantidad = cantidades[i],
                    PrecioUnitario = preciosUnitarios[i]
                };

                _context.Ventadetalle.Add(detalle);

                // Descontar stock
                producto.Stock -= cantidades[i];

                // Calcular total
                venta.Total += cantidades[i] * preciosUnitarios[i];
            }

            _context.SaveChanges();

            TempData["VentaId"] = venta.VentaId;
            return RedirectToAction("Confirmacion");
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
        [HttpPost]
public IActionResult Anular(int id)
{
    var venta = _context.Venta
        .Include(v => v.Ventadetalles)
        .ThenInclude(dv => dv.Producto)
        .FirstOrDefault(v => v.VentaId == id);

    if (venta == null)
    {
        return NotFound();
    }

    if (venta.Anulada)
    {
        TempData["Mensaje"] = "La venta ya estaba anulada.";
        return RedirectToAction(nameof(Index));
    }

    // Marcar como anulada
    venta.Anulada = true;

    // Devolver stock
    foreach (var detalle in venta.Ventadetalles)
    {
        detalle.Producto.Stock += detalle.Cantidad;
        _context.Producto.Update(detalle.Producto);
    }

    _context.Update(venta);
    _context.SaveChanges();

    TempData["Mensaje"] = "Venta anulada y stock devuelto correctamente.";
    return RedirectToAction(nameof(Index));
}


        // ✅ Vista especial para venta con Vue
        public IActionResult Ventavue()
        {
            return View();
        }
        public IActionResult DetalleVue(int id)
{
    ViewBag.VentaId = id;
    return View("VentaDetalleVue");
}


    }
    
}
