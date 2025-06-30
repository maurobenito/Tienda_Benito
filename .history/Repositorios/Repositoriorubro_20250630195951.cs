using System.Collections.Generic;
using System.Linq;
using Tienda_Benito.Models;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioRubro : IRepositorio<Rubro>
    {
        private readonly ProyectoTiendaContext _context;

        public RepositorioRubro(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public int Alta(Rubro entidad)
        {
            _context.Rubro.Add(entidad);
            return _context.SaveChanges();
        }

        public int Baja(int id)
        {
            var entidad = _context.Rubro.Find(id);
            if (entidad != null)
            {
                _context.Rubro.Remove(entidad);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Rubro entidad)
        {
            _context.Rubro.Update(entidad);
            return _context.SaveChanges();
        }

        public IList<Rubro> ObtenerTodos()
        {
            return _context.Rubro.ToList();
        }

        public Rubro ObtenerPorId(int id)
        {
            return _context.Rubro.Find(id);
        }
    }
}
