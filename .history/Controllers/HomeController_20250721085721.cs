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
    public IActionResult ReporteEntreFechas(DateTime desde, DateTime hasta)
    {
        var ventas = _context.Venta
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Ventadetalles).ThenInclude(d => d.Producto)
            .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
            .OrderByDescending(v => v.Fecha)
            .ToList();

        ViewBag.Desde = desde.ToShortDateString();
        ViewBag.Hasta = hasta.ToShortDateString();
        return View("ReporteEntreFechas", ventas);
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
}
