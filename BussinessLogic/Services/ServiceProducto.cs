using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic.Services
{
    public class ServiceProducto
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceProducto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<ProductoDTO>> GetAllProductos()
        {
            try
            {
                IList<Producto> productos = await _unitOfWork.ProductoRepository.GetProductoCategoria();

                IList<ProductoDTO> productoDTO = productos.Adapt<IList<ProductoDTO>>();

                return productoDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


  

    }
}