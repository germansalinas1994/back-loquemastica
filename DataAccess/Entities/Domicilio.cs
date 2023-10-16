using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Domicilio
{
    public int IdDomicilio { get; set; }

    public int? Altura { get; set; }

    public string? Calle { get; set; }

    public string? Aclaracion { get; set; }

    public string? Departamento { get; set; }

    public int IdUsuario { get; set; }

    public string? Domiciliocol { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
