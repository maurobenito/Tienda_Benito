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

        public IEnumerable<Rubro> ObtenerTodos()
        {
            return _context.Rubro.ToList();
        }

        public Rubro? ObtenerPorId(int id)
        {
            return _context.Rubro.Find(id);
        }

        public void Crear(Rubro entidad)
        {
            _context.Rubro.Add(entidad);
            _context.SaveChanges();
        }

        public void Actualizar(Rubro entidad)
        {
            _context.Rubro.Update(entidad);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var rubro = _context.Rubro.Find(id);
            if (rubro != null)
            {
                _context.Rubro.Remove(rubro);
                _context.SaveChanges();
            }
        }
    }
}
