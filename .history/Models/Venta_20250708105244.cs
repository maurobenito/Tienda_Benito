using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("venta")]
[Index("ClienteId", Name = "ClienteId")]
[Index("UsuarioId", Name = "UsuarioId")]
public partial class Ventum
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int VentaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [Precision(12, 2)]
    public decimal Total { get; set; }

    [StringLength(50)]
    public string? TipoVenta { get; set; }

    [Column(TypeName = "int(11)")]
    public int? UsuarioId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("Venta")]
    public virtual Cliente? Cliente { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Venta")]
    public virtual Usuario? Usuario { get; set; }
    public bool Anulada { get; set; } = false;


    [InverseProperty("Venta")]
    public virtual ICollection<Ventadetalle> Ventadetalles { get; set; } = new List<Ventadetalle>();
}
