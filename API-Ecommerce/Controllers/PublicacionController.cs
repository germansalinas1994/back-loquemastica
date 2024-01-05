using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class PublicacionController : GenericController
    {

        //Instancio el service que vamos a usar
        private ServicePublicacion _service;

        //Inyecto el service por el constructor
        public PublicacionController(ServicePublicacion service)
        {
            _service = service;
        }
        // GET: api/values

        [HttpGet]
        [Route("/publicaciones")]
        public async Task<ApiResponse> GetPublicaciones([FromQuery] int sucursal)
        {
            try
            {
                IList<PublicacionDTO> publicaciones = await _service.GetPublicacionesSucursal(sucursal);
                ApiResponse response = new ApiResponse(new { data = publicaciones, cantidadPublicaciones = publicaciones.Count() });
                return response;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }



        }

        [HttpGet]
        [Route("/publicacion/{id}")]
        public async Task<ApiResponse> GetPublicacion(int id)
        {

            try
            {
                if (id == 0)
                {
                    throw new ApiException("Debe ingresar un id de publicacion valido");
                }

                PublicacionDTO publicacion = await _service.GetPublicacionById(id);

                if (publicacion == null)
                {
                    throw new ApiException("No se encontro la publicacion");
                }

                ApiResponse response = new ApiResponse(new { data = publicacion });
                return response;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }

        [HttpPost]
        [Authorize(Policy = "Cliente")]
        [Route("/publicacionesCarrito")]
        public async Task<ApiResponse> GetPublicacionesCarrito([FromBody] List<SearchPublicacionCarritoDTO> publicacionCarrito)
        {
            try
            {
                if (publicacionCarrito == null || publicacionCarrito.Count == 0)
                {
                    throw new ApiException("Debe ingresar al menos una publicacion");
                }
                List<PublicacionDTO> publicaciones = (await _service.GetPublicacionesCarrito(publicacionCarrito)).ToList();

                ApiResponse response = new ApiResponse(new { data = publicaciones });

                return response;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }
        [HttpGet]
        [Route("/publicaciones/{idCategoria}")]
        public async Task<ApiResponse> GetPublicacionesByCategoria(int idCategoria)
        {

            try
            {
                if (idCategoria == 0)
                {
                    throw new ApiException("Debe ingresar un id de categoria valido");
                }

                IList<PublicacionDTO> publicaciones = await _service.GetPublicacionesByCategoria(idCategoria);

                ApiResponse response = new ApiResponse(new { data = publicaciones, cantidadPublicaciones = publicaciones.Count() });

                return response;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

        }

        [HttpGet]
        [Route("/publicacionesRolSucursal")]
        [Authorize(Policy = "Sucursal")]
        public async Task<ApiResponse> GetPublicacionesRolSucursal()
        {
            try
            {
                string user = UserEmailFromJWT();
             


                IList<PublicacionDTO> publicaciones = await _service.GetPublicacionesRolSucursal(user);
                return new ApiResponse(new { data = publicaciones });
            
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }

        [HttpPut]
        [Route("/publicacion")]
        [Authorize(Policy = "Sucursal")]

        public async Task<ApiResponse> EditarPublicacion([FromBody] PublicacionDTO publicacion)
        {
            try
            {
                if (publicacion == null)
                {
                    throw new ApiException("Debe ingresar una publicacion");
                }
                await _service.EditarPublicacion(publicacion);
                return new ApiResponse("La publicacion se edito correctamente");
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

    }
}
}


