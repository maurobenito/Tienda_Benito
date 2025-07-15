using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Benito.Models
{
    public class VentaViewModel
    {
        public int ClienteId { get; set; }

        public int UsuarioId { get; set; }

        [Display(Name = "Total de la Venta")]
        public decimal Total { get; set; }

        [Display(Name = "Productos Vendidos")]
        public List<DetalleVentaItem> Productos { get; set; } = new List<DetalleVentaItem>();
    }

    public class DetalleVentaItem
    {
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}
