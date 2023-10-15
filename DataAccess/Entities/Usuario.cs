using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Telefono { get; set; }

    public int? Dni { get; set; }

    public int IdRol { get; set; }

    public virtual ICollection<Domicilio> Domicilio { get; set; } = new List<Domicilio>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
