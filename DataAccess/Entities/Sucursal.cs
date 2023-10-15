using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Sucursal
{
    public int IdSucursal { get; set; }

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
