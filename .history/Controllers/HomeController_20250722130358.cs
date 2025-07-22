using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;

namespace Tienda_Benito.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProyectoTiendaContext _context;

    public HomeController(ILogger<HomeController> logger, ProyectoTiendaContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
{
    return View();
}

    public IActionResult Reportes()
    {
        return View();
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // 🔍 Reporte: Ventas entre fechas
  [HttpGet]
public IActionResult ReporteEntreFechas(DateTime desde, DateTime hasta, int pagina = 1, int tamPagina = 10)
{
    var query = _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .Include(v => v.Ventadetalles)
        .Where(v => !v.Anulada && v.Fecha >= desde && v.Fecha <= hasta)
        .OrderByDescending(v => v.Fecha);

    var totalRegistros = query.Count();

    var ventas = query
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    ViewBag.Desde = desde.ToShortDateString();
    ViewBag.Hasta = hasta.ToShortDateString();

    ViewBag.TotalAcumulado = query.Sum(v => v.Total);
    ViewBag.TotalCosto = query.Sum(v => v.Ventadetalles.Sum(d => d.Cantidad * d.PrecioUnitario));
    ViewBag.Ganancia = ViewBag.TotalAcumulado - ViewBag.TotalCosto;

    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamPagina);

    // Para mantener los filtros en el paginado:
    ViewBag.DesdeFiltro = desde.ToString("yyyy-MM-dd");
    ViewBag.HastaFiltro = hasta.ToString("yyyy-MM-dd");

    return View(ventas);
}



    // 🔍 Reporte: Ventas por producto (por nombre aproximado)
    [HttpGet]
    public IActionResult ReportePorProducto(string nombreProducto)
    {
        var ventas = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
            .Where(v => v.Ventadetalles.Any(d => d.Producto.Nombre.Contains(nombreProducto)))
            .OrderByDescending(v => v.Fecha)
            .ToList();

        ViewBag.Producto = nombreProducto;
        return View("ReportePorProducto", ventas);
    }

    // 🔍 Reporte: Ventas por cliente (por nombre o apellido)
    [HttpGet]
    public IActionResult ReportePorCliente(string nombreCliente)
    {
        var ventas = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
            .Where(v => v.Cliente.Nombre.Contains(nombreCliente) || v.Cliente.Apellido.Contains(nombreCliente))
            .OrderByDescending(v => v.Fecha)
            .ToList();

        ViewBag.Cliente = nombreCliente;
        return View("ReportePorCliente", ventas);
    }
    public IActionResult ReportePorUsuario(string nombreUsuario)
{
    if (string.IsNullOrEmpty(nombreUsuario))
    {
        // Podrías mostrar un mensaje o devolver la vista vacía
        return View();
    }

    // Supongamos que tienes _context.Usuario con los usuarios y sus ventas
    var ventasPorUsuario = _context.Venta
        .Where(v => v.Usuario.Email.Contains(nombreUsuario) || v.Usuario.Email.Contains(nombreUsuario))
        .Select(v => new {
            v.VentaId,
            v.Fecha,
            v.Total,
            UsuarioNombre = v.Usuario.Email
        })
        .ToList();

    return View(ventasPorUsuario);
}

}
