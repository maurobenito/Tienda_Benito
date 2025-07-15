public IActionResult Index(string nombre = "", string rubro = "", string proveedor = "", int pagina = 1, int tamPagina = 5, string orden = "")
{
    var consulta = _context.Producto
        .Include(p => p.Proveedor)
        .Include(p => p.Rubro)
        .AsQueryable();

    if (!string.IsNullOrEmpty(nombre))
        consulta = consulta.Where(p => p.Nombre.Contains(nombre));

    if (!string.IsNullOrEmpty(rubro))
        consulta = consulta.Where(p => p.Rubro.Nombre.Contains(rubro));

    if (!string.IsNullOrEmpty(proveedor))
        consulta = consulta.Where(p => p.Proveedor.Nombre.Contains(proveedor));

    // Orden dinámico
    consulta = orden switch
    {
        "nombre_desc" => consulta.OrderByDescending(p => p.Nombre),
        "precioCosto" => consulta.OrderBy(p => p.PrecioCosto),
        "precioCosto_desc" => consulta.OrderByDescending(p => p.PrecioCosto),
        "precioVenta" => consulta.OrderBy(p => p.PrecioVenta),
        "precioVenta_desc" => consulta.OrderByDescending(p => p.PrecioVenta),
        "rubro" => consulta.OrderBy(p => p.Rubro.Nombre),
        "rubro_desc" => consulta.OrderByDescending(p => p.Rubro.Nombre),
        "proveedor" => consulta.OrderBy(p => p.Proveedor.Nombre),
        "proveedor_desc" => consulta.OrderByDescending(p => p.Proveedor.Nombre),
        _ => consulta.OrderBy(p => p.Nombre),
    };

    int totalRegistros = consulta.Count();
    int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamPagina);

    var productos = consulta
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();

    ViewBag.NombreFiltro = nombre;
    ViewBag.RubroFiltro = rubro;
    ViewBag.ProveedorFiltro = proveedor;
    ViewBag.PaginaActual = pagina;
    ViewBag.TamanoPagina = tamPagina;
    ViewBag.TotalPaginas = totalPaginas;
    ViewBag.OrdenActual = orden;

    // Combo Rubros
    ViewBag.Rubros = _context.Rubro
        .Select(r => new SelectListItem
        {
            Value = r.Nombre,
            Text = r.Nombre
        }).ToList();

    // Combo Proveedores
    ViewBag.Proveedores = _context.Proveedor
        .Select(p => new SelectListItem
        {
            Value = p.Nombre,
            Text = p.Nombre
        }).ToList();

    return View(productos);
}
