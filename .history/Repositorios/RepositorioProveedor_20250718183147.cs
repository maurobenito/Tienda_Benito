using Tienda_Benito.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioProveedor : IRepositorio<Proveedor>
    {
        private readonly ProyectoTiendaContext _context;

        public RepositorioProveedor(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public int Alta(Proveedor entidad)
        {
            _context.Proveedor.Add(entidad);
            return _context.SaveChanges();
        }

        public int Baja(int id)
        {
            var entidad = _context.Proveedor.Find(id);
            if (entidad != null)
            {
                _context.Proveedor.Remove(entidad);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Proveedor entidad)
        {
            _context.Proveedor.Update(entidad);
            return _context.SaveChanges();
        }

        public IList<Proveedor> ObtenerTodos()
        {
            return _context.Proveedor.ToList();
        }

        public Proveedor ObtenerPorId(int id)
        {
            return _context.Proveedor.FirstOrDefault(p => p.ProveedorId == id);
        }
    }
}
