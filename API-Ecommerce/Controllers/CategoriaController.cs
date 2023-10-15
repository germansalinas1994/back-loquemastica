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
    public class CategoriaController : Controller
    {

        //Instancio el service que vamos a usar
        private ServiceCategoria _service;

        //Inyecto el service por el constructor
        public CategoriaController(ServiceCategoria service)
        {
            _service = service;
        }
        // GET: api/values

        //Metodo para traer todas las categorias
        [HttpGet]
        [Route("/categorias")]
        public async Task<ApiResponse> GetCategorias()
        {
            try
            {
                IList<CategoriaDTO> categorias = await _service.GetAllCategorias();
                ApiResponse response = new ApiResponse(new { data = categorias, cantidadCategorias = categorias.Count() });
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
        [Route("/categoria/{id}")]
        public async Task<ApiResponse> GetCategoriaById(int id)
        {
            if (id == 0)
            {
                throw new ApiException("Debe ingresar un id de categoria valido");
            }

            try
            {
                CategoriaDTO categoria = await _service.GetCategoriaById(id);
                ApiResponse response = new ApiResponse(new { data = categoria });
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


        // POST api/values

        //Metodo para insertar una categoria
        [HttpPost]
        [Route("/categorias")]
        public async Task<CategoriaDTO> PostCategoria([FromBody] CategoriaDTO categoria)
        {
            // _service.PostCategoria(categoria);

            CategoriaDTO cat = await _service.PostCategoria(categoria);
            return cat;
        }



        // PUT api/values/5
        //Metodo para editar una categoria
        [HttpPut]
        [Route("/categoria/{id}")]
        public async Task<CategoriaDTO> Put(int id, [FromBody] CategoriaDTO categoria)
        {
            CategoriaDTO categoriaEditada = await _service.EditarCategoria(categoria);
            return categoriaEditada;
        }

        // DELETE api/values/5
        //Metodo para eliminar una categoria, eliminacion fisica
        [HttpDelete]
        [Route("/categoria/{id}")]
        public async Task Delete(int id)
        {
            await _service.DeleteCategoria(id);
        }

        //Metodo para buscar categorias por algun criterio GetByCriteria

        [HttpGet("/buscarCategoria")]
        public async Task<IList<CategoriaDTO>> BuscarCategorias([FromQuery] string descripcion)
        {
            IList<CategoriaDTO> categorias = await _service.BuscarCategorias(descripcion);
            return categorias;
        }

    }
}

