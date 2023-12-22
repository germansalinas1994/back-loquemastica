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
        [Authorize(Policy = "Cliente")]

        public async Task<ApiResponse> CargarUsuarioAuth0([FromBody] RequestCargarUsuarioDTO usuario)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;



                if (user == null || user == "")
                {
                    throw new ApiException("Email vacío, no se puede encontrar el usuario", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }

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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string email = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (email == null || email == "")
                {
                    throw new ApiException("Email vacío, no se puede encontrar el usuario", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }



                UsuarioDTO usuarioDTO = await _serviceUsuario.GetUsuario(email);
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string email = jwtToken.Claims.First(claim => claim.Type == "email").Value;




                if (string.IsNullOrEmpty(email))
                {
                    throw new ApiException("Email vacío, no se puede encontrar el usuario", (int)HttpStatusCode.BadRequest, "Los campos son incorrectos");
                }
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "No tiene acceso");
                }


                List<DomicilioDTO> domicilios = (await _serviceUsuario.GetDomicilios(user)).ToList();


                if (domicilios.Count == 0)
                {
                    return new ApiResponse(domicilios, (int)HttpStatusCode.NoContent);
                }

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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "No tiene acceso");
                }

                if (domicilio == null)
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "No tiene acceso");
                }

                DomicilioDTO domicilioDTO = await _serviceUsuario.EliminarDomicilio(idDomicilio, user);

                if (domicilioDTO == null)
                {
                    throw new ApiException("No se encontró el domicilio", (int)HttpStatusCode.BadRequest, "");
                }

                return new ApiResponse("Se eliminó correctamente", (int)HttpStatusCode.NoContent);
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "No tiene acceso");
                }
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;

                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "No tiene acceso");
                }
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
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;
                if (user == null || user == "")
                {
                    throw new ApiException("No tiene acceso", (int)HttpStatusCode.Unauthorized, "Los campos son incorrectos");
                }


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

        public async Task<IActionResult> GenerarFacturaPedido(int idPedido)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string user = jwtToken.Claims.First(claim => claim.Type == "email").Value;


                byte[] pdfBytes = await _serviceUsuario.CrearPDF(idPedido);

                if(pdfBytes == null)
                {
                    throw new ApiException("No se pudo generar la factura", (int)HttpStatusCode.BadRequest, "");
                }

                return File(pdfBytes, "application/pdf", "Factura.pdf");

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


