using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPago { get; set; }

    public int? IdTipoPedido { get; set; }

    public int? IdPublicacion { get; set; }

    public int? NroPedido { get; set; }

    public DateTime? FechaAlta { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaBaja { get; set; }

    public int? IdEstadoPedido { get; set; }

    public sbyte? Envio { get; set; }

    public string? Pedidocol { get; set; }

    public virtual ICollection<Envio> EnvioNavigation { get; set; } = new List<Envio>();
}
