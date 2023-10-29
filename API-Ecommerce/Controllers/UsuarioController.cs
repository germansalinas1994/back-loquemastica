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
                    return new ApiResponse("Email vacío, no se puede encontrar el usuario", statusCode: 400);
                }

                UsuarioDTO usuarioDTO = await _serviceUsuario.UpdateUsuario(usuario);

                if (usuarioDTO == null)
                {
                    return new ApiResponse("No se pudo actualizar el usuario", statusCode: 400);
                }

                return new ApiResponse(usuarioDTO, statusCode: 200);
            }
            catch (Exception e)
            {
                ApiResponse apiResponse = new ApiResponse();
                apiResponse.IsError = true;
                apiResponse.Message = "Ocurrió un error interno.";
                apiResponse.StatusCode = 500;


                if (e.Message.Contains("usuario.dni_UNIQUE"))
                {
                    apiResponse.Message = "El DNI ya existe";
                    apiResponse.StatusCode = 409;



                }
                return apiResponse;


            }
        }

        [HttpGet]
        [Route("/getDomicilios")]
        public async Task<ApiResponse> GetDomicilios([FromQuery] string email)
        {
            try
            {
                if (email == null || email == "")
                {
                    return new ApiResponse("Email vacío, no se puede encontrar el usuario", statusCode: 400);
                }


                List<DomicilioDTO> domicilios = (await _serviceUsuario.GetDomicilios(email)).ToList();

                if (domicilios == null)
                {
                    return new ApiResponse("No se pudo encontrar el usuario", statusCode: 400);
                }
                if (domicilios.Count == 0)
                {
                    return new ApiResponse(domicilios, statusCode: 204);
                }

                return new ApiResponse(domicilios, statusCode: 200);
            }
            catch (Exception e)
            {
                ApiResponse apiResponse = new ApiResponse();
                apiResponse.IsError = true;
                apiResponse.Message = "Ocurrió un error interno.";
                apiResponse.StatusCode = 500;



                return apiResponse;


            }
        }





    }
}


