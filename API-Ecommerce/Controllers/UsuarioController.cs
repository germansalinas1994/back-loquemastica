using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : GenericController
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
        [Authorize(Policy = "Cliente")]

        public async Task<ApiResponse> CargarUsuarioAuth0([FromBody] RequestCargarUsuarioDTO usuario)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                UsuarioDTO usuarioDTO = await _serviceUsuario.CargarUsuarioAuth0(user);

                ApiResponse response = new ApiResponse(new { data = "Se ha cargado el usuario correctamente" });

                return response;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }


        }

        [HttpGet]
        [Route("/getUsuario")]
        [Authorize(Policy = "Cliente")]

        public async Task<ApiResponse> GetUsuario()
        {

            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();


                UsuarioDTO usuarioDTO = await _serviceUsuario.GetUsuario(user);
                return new ApiResponse(new { data = usuarioDTO });

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }


        }

        [HttpPost]
        [Route("/actualizarUsuario")]
        [Authorize(Policy = "Cliente")]

        public async Task<ApiResponse> ActualizarUsuario([FromBody] UsuarioDTO usuario)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                if (usuario == null)
                {
                    throw new ApiException("No se pudo actualizar el usuario", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }


                UsuarioDTO usuarioDTO = await _serviceUsuario.UpdateUsuario(usuario);

                if (usuarioDTO == null)
                {
                    throw new ApiException("No se pudo actualizar el usuario", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

                return new ApiResponse(usuarioDTO, (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }

        [HttpGet]
        [Authorize(Policy = "Cliente")]
        [Route("/domicilios")]
        public async Task<ApiResponse> GetDomicilios()
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                List<DomicilioDTO> domicilios = (await _serviceUsuario.GetDomicilios(user)).ToList();

                return new ApiResponse(domicilios, (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }

        [HttpPost]
        [Authorize(Policy = "Cliente")]
        [Route("/domicilio")]
        public async Task<ApiResponse> PostDomicilio([FromBody] DomicilioDTO domicilio)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                if (domicilio == null || domicilio.IdDomicilio != 0)
                {
                    throw new ApiException("No se pudo agregar el domicilio", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

                DomicilioDTO domicilioDTO = await _serviceUsuario.PostDomicilio(domicilio, user);

                if (domicilioDTO == null)
                {
                    throw new ApiException("No se pudo agregar el domicilio", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

                return new ApiResponse(domicilioDTO, (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }
        }


        [HttpPut]
        [Route("/eliminarDomicilio/{idDomicilio}")]
        [Authorize(Policy = "Cliente")]

        public async Task<ApiResponse> EliminarDomicilio(int idDomicilio)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                DomicilioDTO domicilioDTO = await _serviceUsuario.EliminarDomicilio(idDomicilio, user);

                if (domicilioDTO == null)
                {
                    throw new ApiException("No se encontró el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                return new ApiResponse("Se eliminó correctamente", (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }
        }

        [HttpGet]
        [Route("/domicilio")]
        [Authorize(Policy = "Cliente")]
        public async Task<ApiResponse> GetDomicilio([FromQuery] int idDomicilio)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                if (idDomicilio == 0 || idDomicilio == null)
                {
                    throw new ApiException("No se pudo encontrar el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                DomicilioDTO domicilioDTO = await _serviceUsuario.GetDomicilio(idDomicilio, user);

                if (domicilioDTO == null)
                {
                    throw new ApiException("No se pudo encontrar el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                return new ApiResponse(domicilioDTO, (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }

        [HttpPut]
        [Route("/editarDomicilio")]
        [Authorize(Policy = "Cliente")]
        public async Task<ApiResponse> EditarDomicilio([FromBody] DomicilioDTO domicilio)
        {
            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();

                if (domicilio.IdDomicilio == 0 || domicilio.IdDomicilio == null)
                {
                    throw new ApiException("No se pudo editar el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                DomicilioDTO domicilioDTO = await _serviceUsuario.EditarDomicilio(domicilio, user);

                if (domicilioDTO == null)
                {
                    throw new ApiException("No se pudo editar el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                return new ApiResponse(domicilioDTO, (int)HttpStatusCode.OK);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }


        [HttpGet]
        [Authorize(Policy = "Cliente")]
        [Route("/pedidos")]
        public async Task<ApiResponse> GetPedidos()
        {
            ApiResponse apiResponse = new ApiResponse();

            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();



                List<PedidoDTO> pedidos = await _serviceUsuario.GetPedidos(user);

                if (pedidos == null)
                {
                    throw new ApiException("No se encontraron pedidos", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

                return new ApiResponse(pedidos, (int)HttpStatusCode.OK);

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }



        [HttpGet]
        [Authorize(Policy = "Cliente")]
        [Route("/generarFactura/{idPedido}")]
        public async Task<ApiResponse> GenerarFacturaPedido(int idPedido)
        {
            try
            {
                byte[] pdfBytes = await _serviceUsuario.CrearPDF(idPedido);

                if (pdfBytes == null)
                {
                    throw new ApiException("No se pudo generar la factura", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

                // Convertir el PDF a Base64
                string pdfBase64 = Convert.ToBase64String(pdfBytes);

                // Devolver la cadena Base64 como parte de la respuesta
                return new ApiResponse(new { pdf = pdfBase64 }, (int)HttpStatusCode.OK);

                // return Ok(new { pdf = pdfBase64 });
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }



    }
}


