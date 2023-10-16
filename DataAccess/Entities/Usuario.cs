using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string Email { get; set; } = null!;

    public int? Dni { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Domicilio> Domicilio { get; set; } = new List<Domicilio>();
}
