// RepositorioUsuario.cs
using Tienda_Benito.Models;
using System.Collections.Generic;
using System.Linq;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioUsuario : IRepositorio<Usuario>
    {
        private readonly ProyectoTiendaContext context;

        public RepositorioUsuario(ProyectoTiendaContext context)
        {
            this.context = context;
        }

        public int Alta(Usuario u)
        {
            context.Usuario.Add(u);
            return context.SaveChanges();
        }

        public int Baja(int id)
        {
            var u = context.Usuario.Find(id);
            if (u != null)
            {
                context.Usuario.Remove(u);
                return context.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Usuario u)
        {
            context.Usuario.Update(u);
            return context.SaveChanges();
        }

        public Usuario? ObtenerPorId(int id)
        {
            return context.Usuario.Find(id);
        }

        public IList<Usuario> ObtenerTodos()
        {
            return context.Usuario.ToList();
        }
    }
}
