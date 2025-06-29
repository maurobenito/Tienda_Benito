using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tienda_Benito.Models;

[Table("rubro")]
public partial class Rubro
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int RubroId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;
}
