using System.Collections.Generic;

namespace Tienda_Benito.Repositorios
{
    public interface IRepositorio<T>
    {
        int Alta(T entidad);
        int Baja(int id);
        int Modificacion(T entidad);
        IList<T> ObtenerTodos();
        T ObtenerPorId(int id);
    }
}
