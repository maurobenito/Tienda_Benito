using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("producto")]
public partial class Producto
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ProductoId { get; set; }

    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Descripcion { get; set; }

    [Precision(12, 2)]
    public decimal PrecioCosto { get; set; }

    [Precision(12, 2)]
    public decimal PrecioVenta { get; set; }

    [StringLength(50)]
    public string? UnidadMedida { get; set; }

    [Column(TypeName = "int(11)")]
    public int? RubroId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ProveedorId { get; set; }

    [InverseProperty("Producto")]
    public virtual ICollection<Ventadetalle> Ventadetalles { get; set; } = new List<Ventadetalle>();
}
