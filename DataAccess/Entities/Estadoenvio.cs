using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Estadoenvio
{
    public int IdEstadoEnvio { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Envio> Envio { get; set; } = new List<Envio>();
}
