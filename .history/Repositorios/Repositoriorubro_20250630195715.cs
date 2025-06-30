using Tienda_Benito.Models;

namespace Tienda_Benito.Repositorios
{
    public interface RepositorioRubro
    {
        IEnumerable<Rubro> ObtenerTodos();
        Rubro? ObtenerPorId(int id);
        void Crear(Rubro rubro);
        void Editar(Rubro rubro);
        void Eliminar(int id);
    }
}
