using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("proveedor")]
public partial class Proveedor
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ProveedorId { get; set; }

    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Telefono { get; set; }

    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("CUIT")]
    [StringLength(50)]
    public string? Cuit { get; set; }

    [StringLength(150)]
    public string? RazonSocial { get; set; }

    [StringLength(255)]
    public string? DatosBancarios { get; set; }
    [StringLength(255)]
public string? Archivo1 { get; set; }

[StringLength(255)]
public string? Archivo2 { get; set; }
}
