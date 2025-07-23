using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]

    public IActionResult Index()
    {
        return View();
    }
[Authorize(Roles = "Admin")]
    public IActionResult Reportes()
    {
        return View();
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

[Authorize(Roles = "Admin")]
   [HttpGet]
public IActionResult ReporteEntreFechas(DateTime desde, DateTime hasta)
{
    var ventas = _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .Include(v => v.Ventadetalles)
            .ThenInclude(d => d.Producto)
        .Where(v => !v.Anulada && v.Fecha >= desde && v.Fecha <= hasta)
        .OrderByDescending(v => v.Fecha)
        .ToList();

    var totalAcumulado = ventas.Sum(v => v.Total);
    var totalCosto = ventas.Sum(v => v.Ventadetalles.Sum(d => d.Cantidad * d.Producto.PrecioCosto));
    var ganancia = totalAcumulado - totalCosto;

    ViewBag.Desde = desde.ToShortDateString();
    ViewBag.Hasta = hasta.ToShortDateString();
    ViewBag.TotalAcumulado = totalAcumulado;
    ViewBag.TotalCosto = totalCosto;
    ViewBag.Ganancia = ganancia;

    return View("ReporteEntreFechas", ventas);
}


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult ReportePorProducto(string nombreProducto)
    {
        var ventas = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
            .Where(v => v.Ventadetalles.Any(d => d.Producto.Nombre.Contains(nombreProducto)))
            .OrderByDescending(v => v.Fecha)
            .ToList();
        var totalAcumulado = ventas.Sum(v => v.Total);
        var totalCosto = ventas.Sum(v => v.Ventadetalles.Sum(d => d.Cantidad * d.Producto.PrecioCosto));
        var ganancia = totalAcumulado - totalCosto;

        ViewBag.Producto = nombreProducto;
        ViewBag.TotalAcumulado = totalAcumulado;
        ViewBag.TotalCosto = totalCosto;
        ViewBag.Ganancia = ganancia;

        return View("ReportePorProducto", ventas);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult ReportePorCliente(string nombreCliente)
    {
        var ventas = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
            .Where(v => v.Cliente.Nombre.Contains(nombreCliente) || v.Cliente.Apellido.Contains(nombreCliente))
            .OrderByDescending(v => v.Fecha)
            .ToList();
        var totalAcumulado = ventas.Sum(v => v.Total);
        var totalCosto = ventas.Sum(v => v.Ventadetalles.Sum(d => d.Cantidad * d.Producto.PrecioCosto));
        var ganancia = totalAcumulado - totalCosto;

        ViewBag.Cliente = nombreCliente;
        ViewBag.TotalAcumulado = totalAcumulado;
        ViewBag.TotalCosto = totalCosto;
        ViewBag.Ganancia = ganancia;

        return View("ReportePorCliente", ventas);
    }
    [Authorize(Roles = "Admin")]
 [HttpGet]
public IActionResult ReportePorUsuario(string nombreUsuario)
{
    if (string.IsNullOrEmpty(nombreUsuario))
    {
        ViewBag.TotalAcumulado = 0;
        ViewBag.TotalCosto = 0;
        ViewBag.Ganancia = 0;
        ViewBag.Usuario = "";
        return View("ReportePorUsuario", new List<Ventum>());
    }

    var ventas = _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
        .Where(v => v.Usuario.Email.Contains(nombreUsuario))
        .OrderByDescending(v => v.Fecha)
        .ToList();

    var totalAcumulado = ventas.Sum(v => v.Total);
    var totalCosto = ventas.Sum(v => v.Ventadetalles.Sum(d => d.Cantidad * d.Producto.PrecioCosto));
    var ganancia = totalAcumulado - totalCosto;

    ViewBag.Usuario = nombreUsuario;
    ViewBag.TotalAcumulado = totalAcumulado;
    ViewBag.TotalCosto = totalCosto;
    ViewBag.Ganancia = ganancia;

    return View("ReportePorUsuario", ventas);
}

[HttpGet]
public IActionResult AccessDenied()
{
    return View();
}

}
