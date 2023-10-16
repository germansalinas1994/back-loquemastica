﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Sucursal
{
    public int IdSucursal { get; set; }

    public string? Direccion { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Publicacion> Publicacion { get; set; } = new List<Publicacion>();
}
