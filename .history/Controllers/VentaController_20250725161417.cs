using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;

namespace Tienda_Benito.Controllers
{
    public class VentaController : Controller
    {
        private readonly RepositorioVenta _repositorio; // ✅ Cambio de tipo
        private readonly ProyectoTiendaContext _context;

        public VentaController(RepositorioVenta repositorio, ProyectoTiendaContext context)
        {
            _repositorio = repositorio;
            _context = context;
        }
public IActionResult Index(string cliente, string vendedor, string tipo, string orderBy = "fecha", bool desc = true, int pagina = 1, int tamPagina = 5)
{
    // Detectar si viene con "_desc"
    if (orderBy != null && orderBy.EndsWith("_desc"))
    {
        orderBy = orderBy[..^5];
        desc = true;
    }
    else
    {
        desc = false;
    }

    var query = _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .AsQueryable();

    if (!string.IsNullOrEmpty(cliente))
        query = query.Where(v => (v.Cliente.Nombre + " " + v.Cliente.Apellido).ToLower().Contains(cliente.ToLower()));

    if (!string.IsNullOrEmpty(vendedor))
        query = query.Where(v => v.Usuario.Email.ToLower().Contains(vendedor.ToLower()));

    if (!string.IsNullOrEmpty(tipo))
        query = query.Where(v => v.TipoVenta.ToLower().Contains(tipo.ToLower()));

    query = (orderBy?.ToLower(), desc) switch
    {
        ("cliente", true) => query.OrderByDescending(v => v.Cliente.Nombre),
        ("cliente", false) => query.OrderBy(v => v.Cliente.Nombre),
        ("vendedor", true) => query.OrderByDescending(v => v.Usuario.Email),
        ("vendedor", false) => query.OrderBy(v => v.Usuario.Email),
        ("tipo", true) => query.OrderByDescending(v => v.TipoVenta),
        ("tipo", false) => query.OrderBy(v => v.TipoVenta),
        ("total", true) => query.OrderByDescending(v => v.Total),
        ("total", false) => query.OrderBy(v => v.Total),
        ("fecha", false) => query.OrderBy(v => v.Fecha),
        _ => query.OrderByDescending(v => v.Fecha)
    };

    var total = query.Count();

    var ventas = query
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPagina);
    ViewBag.Cliente = cliente;
    ViewBag.Vendedor = vendedor;
    ViewBag.Tipo = tipo;
    ViewBag.OrderBy = orderBy;
    ViewBag.Desc = desc;

    return View(ventas);
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

                producto.Stock -= cantidades[i];
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

            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Email", venta.UsuarioId);
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

            ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Email", v.UsuarioId);
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

    venta.Anulada = true;

    foreach (var detalle in venta.Ventadetalles)
    {
        // Devolver stock al producto vendido
        detalle.Producto.Stock += detalle.Cantidad;
        _context.Producto.Update(detalle.Producto);

        // Si es un producto fraccionado, devolver también al padre
        if (detalle.Producto.ProductoPadreId.HasValue && detalle.Producto.EquivalenciaEnPadre.HasValue)
        {
            var productoPadre = _context.Producto.FirstOrDefault(p => p.ProductoId == detalle.Producto.ProductoPadreId.Value);
            if (productoPadre != null)
            {
                // Devolver stock al padre
                productoPadre.Stock += detalle.Cantidad * detalle.Producto.EquivalenciaEnPadre.Value;
                _context.Producto.Update(productoPadre);

                // ✅ Recalcular stock del hijo basado en el stock actual del padre
                detalle.Producto.Stock = (int)(productoPadre.Stock / detalle.Producto.EquivalenciaEnPadre.Value);
                _context.Producto.Update(detalle.Producto);
            }
        }
    }

    _context.Venta.Update(venta);
    _context.SaveChanges();

    TempData["Mensaje"] = "La venta fue anulada exitosamente.";
    return RedirectToAction(nameof(Index));
}


        // ✅ Vista Vue
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
