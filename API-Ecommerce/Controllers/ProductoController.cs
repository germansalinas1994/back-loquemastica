using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class ProductoController : Controller
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
        public async Task<ApiResponse> GetProductos()
        {
            try
            {
                IList<ProductoDTO> productos = await _service.GetAllProductos();
                ApiResponse response = new ApiResponse(new { data = productos, cantidadProductos = productos.Count() });
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

        [HttpPut]
        [Route("/producto/{id}")]

        public async Task<ApiResponse> EliminarProducto(int id)
        {
            await _service.EliminarProducto(id);
            ApiResponse response = new ApiResponse("El PID se eliminó exitosamente");
            return response;
        }

        [HttpGet]
        [Route("/producto/{id}")]

        public async Task<ApiResponse> GetProductoById(int id)
        {
            try
            {
                ProductoDTO producto = await _service.GetProductoById(id);
                ApiResponse response = new ApiResponse(new { data = producto });
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

        //Metodo para cargar productos
        [HttpPost]
        [Route("/producto")]
        public async Task<ApiResponse> CargarProducto([FromBody] ProductoDTO producto)
        {
                await _service.CargarProducto(producto);
                ApiResponse response = new ApiResponse("El producto se cargó exitosamente");
                return response;
        }

        [HttpPut]
        [Route("/producto")]
        public async Task<ApiResponse> EditarProducto([FromBody] ProductoDTO producto)
        {
            await _service.EditarProducto(producto);
            ApiResponse response = new ApiResponse("El Producto se modificó exitosamente");
            return response;
        }



    }
}


