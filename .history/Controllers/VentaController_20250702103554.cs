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
    ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre");
    ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Email");
    ViewBag.Productos = new SelectList(_context.Producto, "ProductoId", "Nombre");

    var model = new VentaViewModel();
    return View(model);
}

[HttpPost]
public IActionResult Create(VentaViewModel vm)
{
    if (ModelState.IsValid)
    {
        decimal totalVenta = 0;

        foreach (var detalle in vm.Detalles)
        {
            var producto = _context.Producto.FirstOrDefault(p => p.ProductoId == detalle.ProductoId);
            if (producto == null)
                return BadRequest("Producto no encontrado");

            if (producto.Stock < detalle.Cantidad)
                return BadRequest($"Stock insuficiente para el producto: {producto.Nombre}");

            // Descontar stock
            producto.Stock -= detalle.Cantidad;

            // Asignar precio de venta actual
            detalle.PrecioUnitario = producto.PrecioVenta;

            // Calcular subtotal
            totalVenta += detalle.Cantidad * detalle.PrecioUnitario;

            // Asignar venta (se hace después de guardar venta)
            detalle.Venta = vm.Venta;
        }

        vm.Venta.Fecha = DateTime.Now;
        vm.Venta.Total = totalVenta;

        _context.Venta.Add(vm.Venta);
        _context.SaveChanges();

        foreach (var detalle in vm.Detalles)
        {
            detalle.VentaId = vm.Venta.VentaId; // ahora sí tiene ID
            _context.Ventadetalle.Add(detalle);
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    ViewBag.Clientes = new SelectList(_context.Cliente, "ClienteId", "Nombre", vm.Venta.ClienteId);
    ViewBag.Usuarios = new SelectList(_context.Usuario, "UsuarioId", "Email", vm.Venta.UsuarioId);
    ViewBag.Productos = new SelectList(_context.Producto, "ProductoId", "Nombre");

    return View(vm);
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
