using Tienda_Benito.Models;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioProducto : IRepositorio<Producto>
    {
        private readonly ProyectoTiendaContext contexto;

        public RepositorioProducto(ProyectoTiendaContext contexto)
        {
            this.contexto = contexto;
        }

        public int Alta(Producto entidad)
        {
            contexto.Producto.Add(entidad);
            return contexto.SaveChanges();
        }

        public int Baja(int id)
        {
            var entidad = contexto.Producto.Find(id);
            if (entidad != null)
            {
                contexto.Producto.Remove(entidad);
                return contexto.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Producto entidad)
        {
            contexto.Producto.Update(entidad);
            return contexto.SaveChanges();
        }

        public IList<Producto> ObtenerTodos()
        {
            return contexto.Producto
                .Include(p => p.Rubro)       // Propiedad navegación, no RubroId
                .Include(p => p.Proveedor)   // Propiedad navegación, no ProveedorId
                .ToList();
        }

        public Producto? ObtenerPorId(int id)
        {
            return contexto.Producto
                .Include(p => p.Rubro)
                .Include(p => p.Proveedor)
                .FirstOrDefault(p => p.ProductoId == id);
        }
    }
}
