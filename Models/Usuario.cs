using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("usuario")]
public partial class Usuario
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int UsuarioId { get; set; }

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(50)]
    public string Rol { get; set; } = null!;

    [StringLength(255)]
    public string? Avatar { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
