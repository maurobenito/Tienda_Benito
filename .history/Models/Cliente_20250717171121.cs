using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("cliente")]
public partial class Cliente
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ClienteId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string Apellido { get; set; } = null!;

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Telefono { get; set; }

    [StringLength(255)]
    public string? Direccion { get; set; }

    [StringLength(50)]
    public string? CondFiscal { get; set; }

    [Column("CUIT")]
    [StringLength(50)]
    public string? Cuit { get; set; }

    [Column(TypeName = "int(11)")]
    
    public int? UsuarioId { get; set; }
    [StringLength(255)]
public string? ArchivoAdjunto { get; set; }

[NotMapped]
public IFormFile? ArchivoSubido { get; set; }

    [InverseProperty("Cliente")]
    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
