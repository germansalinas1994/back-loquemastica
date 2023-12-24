using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class ProductoController : GenericController
    {

        //Instancio el service que vamos a usar
        private ServiceProducto _service;


        //Inyecto el service por el constructor
        public ProductoController(ServiceProducto service)
        {
            _service = service;
        }
        // GET: api/values

        //Metodo para traer todas las categorias
        [HttpGet]
        [Route("/productos")]
        [Authorize(Policy = "Admin")]
        public async Task<ApiResponse> GetProductos()
        {
            try
            {
                // string user = UserEmailFromJWT();

                IList<ProductoDTO> productos = await _service.GetAllProductos();
                ApiResponse response = new ApiResponse(new { data = productos, cantidadProductos = productos.Count() });
                return response;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }

        [HttpPut]
        [Route("/producto/{id}")]
        [Authorize(Policy = "Admin")]


        public async Task<ApiResponse> EliminarProducto(int id)
        {
            try
            {
                await _service.EliminarProducto(id);
                return new ApiResponse("El producto se eliminó exitosamente");
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
        [Route("/producto/{id}")]
        [Authorize(Policy = "Admin")]

        public async Task<ApiResponse> GetProductoById(int id)
        {
            try
            {
                ProductoDTO producto = await _service.GetProductoById(id);
                return new ApiResponse(new { data = producto });
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

        //Metodo para cargar productos
        [HttpPost]
        [Route("/producto")]
        [Authorize(Policy = "Admin")]
        public async Task<ApiResponse> CargarProducto([FromBody] ProductoDTO producto)
        {
            try
            {
                await _service.CargarProducto(producto);
                return new ApiResponse("El producto se cargó exitosamente");

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
        [Route("/producto")]
        [Authorize(Policy = "Admin")]
        public async Task<ApiResponse> EditarProducto([FromBody] ProductoDTO producto)
        {
            try
            {
                await _service.EditarProducto(producto);
                return new ApiResponse("El producto se modificó exitosamente");

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


