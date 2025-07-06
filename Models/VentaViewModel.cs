using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Benito.Models
{
    public class VentaViewModel
    {
        public Ventum Venta { get; set; } = new Ventum();

        [Display(Name = "Detalles de Venta")]
        public List<Ventadetalle> Detalles { get; set; } = new List<Ventadetalle>();
    }
}
