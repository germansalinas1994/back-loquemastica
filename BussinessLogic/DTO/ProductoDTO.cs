using System;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace BussinessLogic.DTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }

        public string? Descripcion { get; set; }

        public decimal? Precio { get; set; }


        public int? idCategoria { get; set; }

        public string? UrlImagen { get; set; }

        public string Nombre { get; set; }
        public CategoriaDTO? IdCategoriaNavigation { get; set; }
<<<<<<< HEAD
=======

        public IFormFile? Archivo { get; set; }

>>>>>>> 00bc07b643cd90c02a3c4f189645c9ce73901536


    }
}

