using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {

        //Instancio el service que vamos a usar
        private ServiceUsuario _serviceUsuario;

        //Inyecto el service por el constructor
        public UsuarioController(ServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }



        [HttpPost]
        [Route("/cargarUsuario")]
        public async Task<ApiResponse> CargarUsuarioAuth0([FromBody] RequestCargarUsuarioDTO usuario)
        {
            try
            {
                if (usuario.Email == null || usuario.Email == "")
                {
                    return new ApiResponse("Email vacío");
                }

                UsuarioDTO usuarioDTO = await _serviceUsuario.CargarUsuarioAuth0(usuario);

                ApiResponse response = new ApiResponse(new { data = "Se ha cargado el usuario correctamente" });

                return response;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw new ApiException(ex);
            }

        }

        [HttpGet]
        [Route("/getUsuario")]
        public async Task<ApiResponse> GetUsuario([FromQuery] string email)
        {
            try
            {
                if (email == null || email == "")
                {
                     return new ApiResponse("Email vacío, no se puede encontrar el usuario");
                }

                try
                {

                    UsuarioDTO usuarioDTO = await _serviceUsuario.GetUsuario(email);
                    ApiResponse response = new ApiResponse(new { data = usuarioDTO });
                    return response;


                }
                catch (Exception e)
                {
                    return new ApiResponse(new { data = "Hubo problemas para encontrar el usuario " });
                }
                // UsuarioDTO usuarioDTO = await _serviceUsuario.CargarUsuarioAuth0(usuario);


            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw new ApiException(ex);
            }

        }

         [HttpPost]
        [Route("/actualizarUsuario")]
        public async Task<ApiResponse> ActualizarUsuario([FromBody] UsuarioDTO usuario)
        {
            try
            {
                if (usuario.Email == null || usuario.Email == "")
                {
                     return new ApiResponse("Email vacío, no se puede encontrar el usuario");
                }

                try
                {

                    UsuarioDTO usuarioDTO = await _serviceUsuario.UpdateUsuario(usuario);

                    if(usuarioDTO == null){
                        return new ApiResponse("No se pudo actualizar el usuario");
                    }

                    ApiResponse response = new ApiResponse(new { data = usuarioDTO });
                    return response;


                }
                catch (Exception e)
                {
                    return new ApiResponse(new { data = "Hubo problemas para encontrar el usuario " });
                }
                // UsuarioDTO usuarioDTO = await _serviceUsuario.CargarUsuarioAuth0(usuario);


            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw new ApiException(ex);
            }

        }






    }
}


