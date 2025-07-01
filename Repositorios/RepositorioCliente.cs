using Tienda_Benito.Models;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioCliente : IRepositorio<Cliente>
    {
        private readonly ProyectoTiendaContext _context;

        public RepositorioCliente(ProyectoTiendaContext context)
        {
            _context = context;
        }

        public int Alta(Cliente entidad)
        {
            _context.Cliente.Add(entidad);
            return _context.SaveChanges();
        }

        public int Baja(int id)
        {
            var cliente = _context.Cliente.Find(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
                return _context.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Cliente entidad)
        {
            _context.Cliente.Update(entidad);
            return _context.SaveChanges();
        }

        public Cliente ObtenerPorId(int id)
        {
            return _context.Cliente.FirstOrDefault(c => c.ClienteId == id);
        }

        public IList<Cliente> ObtenerTodos()
        {
            return _context.Cliente.ToList();
        }
    }
}
