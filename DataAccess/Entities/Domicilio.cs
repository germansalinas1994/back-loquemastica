using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Domicilio
{
    public int IdDomicilio { get; set; }

    public int Altura { get; set; }

    public string Calle { get; set; } = null!;

    public int IdUsuario { get; set; }

    public virtual ICollection<Envio> Envio { get; set; } = new List<Envio>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
