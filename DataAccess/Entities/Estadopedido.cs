using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Estadopedido
{
    public int IdEstadoPedido { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
