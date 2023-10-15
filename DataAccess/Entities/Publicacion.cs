using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Publicacion
{
    public int IdPublicacion { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public float? Precio { get; set; }

    public int IdProducto { get; set; }

    public int? IdEstado { get; set; }

    public int Stock { get; set; }

    public virtual Estadopublicacion? IdEstadoNavigation { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
