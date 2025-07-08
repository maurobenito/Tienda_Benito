using System.Collections.Generic;
using System.Linq;
using Tienda_Benito.Models;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioVenta : IRepositorio<Ventum>
    {
        private readonly ProyectoTiendaContext _context;

        public RepositorioVenta(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public int Alta(Ventum entidad)
        {
            _context.Venta.Add(entidad);
            return _context.SaveChanges();
        }

        public int Baja(int id)
        {
            var venta = _context.Venta.Find(id);
            if (venta != null)
            {
                _context.Venta.Remove(venta);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Ventum entidad)
        {
            _context.Venta.Update(entidad);
            return _context.SaveChanges();
        }

        public IList<Ventum> ObtenerTodos()
        {
            return _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .ToList();
        }

        public Ventum ObtenerPorId(int id)
        {
            return _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefault(v => v.VentaId == id);
        }
        public List<Ventum> ObtenerPaginado(int pagina, int tamPagina)
{
    return _context.Venta
        .Include(v => v.Cliente)
        .Include(v => v.Usuario)
        .OrderByDescending(v => v.Fecha)
        .Skip((pagina - 1) * tamPagina)
        .Take(tamPagina)
        .ToList();
}

public int Contar()
{
    return _context.Venta.Count();
}

    }
}
