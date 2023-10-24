using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessLogic.DTO.Search
{
    public class RequestCargarUsuarioDTO
    {
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string ImagenUsuario { get; set; }
    }
}