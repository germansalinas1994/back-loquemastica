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



    }
}

