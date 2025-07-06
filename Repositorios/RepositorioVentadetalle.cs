using System.Collections.Generic;
using System.Linq;
using Tienda_Benito.Models;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Repositorios
{
    public class RepositorioVentadetalle : IRepositorio<Ventadetalle>
    {
        private readonly ProyectoTiendaContext contexto;

        public RepositorioVentadetalle(ProyectoTiendaContext contexto)  // <-- nombre igual a clase
        {
            this.contexto = contexto;
        }

        public int Alta(Ventadetalle entidad)
        {
            contexto.Ventadetalle.Add(entidad);
            return contexto.SaveChanges();
        }

        public int Baja(int id)
        {
            var entidad = contexto.Ventadetalle.Find(id);
            if (entidad != null)
            {
                contexto.Ventadetalle.Remove(entidad);
                return contexto.SaveChanges();
            }
            return 0;
        }

        public int Modificacion(Ventadetalle entidad)
        {
            contexto.Ventadetalle.Update(entidad);
            return contexto.SaveChanges();
        }

        public IList<Ventadetalle> ObtenerTodos()
        {
            return contexto.Ventadetalle
                .Include(v => v.Producto)
                .Include(v => v.Venta)
                .ToList();
        }

        public Ventadetalle ObtenerPorId(int id)
        {
            return contexto.Ventadetalle
                .Include(v => v.Producto)
                .Include(v => v.Venta)
                .FirstOrDefault(v => v.VentaDetalleId == id);
        }
    }
}
