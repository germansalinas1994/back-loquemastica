using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Envio
{
    public int IdEnvio { get; set; }

    public int IdPedido { get; set; }

    public string CodigoSeguimiento { get; set; } = null!;

    public int IdDomicilio { get; set; }

    public virtual Domicilio IdDomicilioNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
