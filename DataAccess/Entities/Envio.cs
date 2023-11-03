using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Envio
{
    public int IdEnvio { get; set; }

    public int? IdEstadoEnvio { get; set; }

    public float? Precio { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public virtual Estadoenvio? IdEstadoEnvioNavigation { get; set; }

    public virtual Pedido? Pedido { get; set; } 
}
