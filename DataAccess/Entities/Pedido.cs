using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public string? Token { get; set; }

    public int IdPublicacion { get; set; }

    public int Cantidad { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoEntrega { get; set; }

    public int IdEstadoPedido { get; set; }

    public int? IdSucursal { get; set; }

    public virtual Envio? Envio { get; set; }

    public virtual Estadopedido IdEstadoPedidoNavigation { get; set; } = null!;

    public virtual Publicacion IdPublicacionNavigation { get; set; } = null!;

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
