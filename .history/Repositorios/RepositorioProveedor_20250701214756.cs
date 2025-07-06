using Tienda_Benito.Models;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioProveedor : IRepositorioProveedor
    {
        private readonly ProyectoTiendaContext contexto;

        public RepositorioProveedor(ProyectoTiendaContext contexto)
        {
            this.contexto = contexto;
        }

        public int Alta(Proveedor entidad)
        {
            contexto.Proveedor.Add(entidad);
            return contexto.SaveChanges();
        }

        public int Baja(int id)
        {
            var entidad = contexto.Proveedor.Find(id);
            if (entidad != null)
            {
                contexto.Proveedor.Remove(entidad);
                return contexto.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Proveedor entidad)
        {
            contexto.Proveedor.Update(entidad);
            return contexto.SaveChanges();
        }

        public IList<Proveedor> ObtenerTodos()
        {
            return contexto.Proveedor.ToList();
        }

        public Proveedor ObtenerPorId(int id)
        {
            return contexto.Proveedor.FirstOrDefault(p => p.ProveedorId == id);
        }
    }
}
