using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("ventadetalle")]
[Index("ProductoId", Name = "ProductoId")]
[Index("VentaId", Name = "VentaId")]
public partial class Ventadetalle
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int VentaDetalleId { get; set; }

    [Column(TypeName = "int(11)")]
    public int VentaId { get; set; }

    [Column(TypeName = "int(11)")]
    public int ProductoId { get; set; }

    [Column(TypeName = "int(11)")]
    public int Cantidad { get; set; }

    [Precision(12, 2)]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("Ventadetalles")]
    public virtual Producto Producto { get; set; } = null!;

    [ForeignKey("VentaId")]
    [InverseProperty("Ventadetalles")]
    public virtual Ventum Venta { get; set; } = null!;
}
