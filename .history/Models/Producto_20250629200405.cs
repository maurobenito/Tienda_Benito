using System.Collections.Generic;

namespace Tienda_Benito.Models
{
    public partial class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public string? UnidadMedida { get; set; }

        public int? ProveedorId { get; set; }
        public int? RubroId { get; set; }

        // ✅ Propiedades de navegación necesarias
       public Proveedor? Proveedor { get; set; }
        public Rubro? Rubro { get; set; }

        public virtual ICollection<Ventadetalle> Ventadetalles { get; set; } = new List<Ventadetalle>();
    }
}
