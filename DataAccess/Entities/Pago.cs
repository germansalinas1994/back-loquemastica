using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Pago
{
    public int IdPago { get; set; }

    public DateTime FechaAlta { get; set; }

    public int NroTransaccion { get; set; }
}
