using System;
using DataAccess.Entities;

namespace BussinessLogic.DTO
{

    public class DatosReporteDTO
    {
        public string NombreSucursal { get; set; }

        public string DireccionSucursal { get; set; }

        public int MesReporte { get; set; }

        public int AnioReporte { get; set; }

        public DateTime FechaReporte { get{
            return new DateTime(this.AnioReporte, this.MesReporte, 1);
        }}


        public List<PedidosReporteDTO> Pedidos { get; set; } = new List<PedidosReporteDTO>();
    }
    public class PedidosReporteDTO
    {
        public decimal? Total { get; set; }

        public long Orden_MercadoPago { get; set; }

        public virtual string EmailUsuario { get; set; }

        public virtual string? EstadoEnvio { get; set; }
        public DateTime FechaPedido { get; set; }


    }
}

