﻿using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using AutoWrapper.Wrappers;
using System.Net;



namespace BussinessLogic.Services
{
    public class ServiceUsuario
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;
        private ServiceReporte _serviceReporte;


        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceUsuario(IUnitOfWork unitOfWork, ServiceReporte serviceReporte)
        {
            _unitOfWork = unitOfWork;
            _serviceReporte = serviceReporte;
        }


        public async Task<string> CargarUsuarioAuth0(string email)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == email)).FirstOrDefault();

                //si encuentra el usuario es que ya esta cargado sino lo cargo

                if (findUsuario != null)
                {
                    return "El usuario ya esta cargado";
                }
                else
                {
                    Usuario nuevoUsuario = new Usuario();
                    nuevoUsuario.Email = email;
                    nuevoUsuario.FechaAlta = DateTime.Now;
                    nuevoUsuario.FechaModificacion = DateTime.Now;

                    await _unitOfWork.UsuarioRepository.Insert(nuevoUsuario);
                    await _unitOfWork.CommitAsync();

                    return "Usuario cargado correctamente";

                }
            }
            catch(ApiException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }


        public async Task<UsuarioDTO> GetUsuario(string email)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == email)).FirstOrDefault();

                if (findUsuario != null)
                {
                    return findUsuario.Adapt<UsuarioDTO>();
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UsuarioDTO> UpdateUsuario(UsuarioDTO usuario)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == usuario.Email)).FirstOrDefault();

                if (findUsuario != null)
                {
                    findUsuario.Nombre = usuario.Nombre;
                    findUsuario.Apellido = usuario.Apellido;
                    findUsuario.Dni = usuario.Dni;
                    findUsuario.Telefono = usuario.Telefono;
                    findUsuario.FechaModificacion = DateTime.Now;

                    await _unitOfWork.UsuarioRepository.Update(findUsuario);
                    await _unitOfWork.CommitAsync();

                    return findUsuario.Adapt<UsuarioDTO>();
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IList<DomicilioDTO>> GetDomicilios(string email)
        {


            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == email)).FirstOrDefault();

                if (findUsuario != null)
                {

                    IList<Domicilio> domicilios = (await _unitOfWork.DomicilioRepository.GetByCriteria(x => x.IdUsuario == findUsuario.IdUsuario && x.FechaHasta == null)).OrderByDescending(x => x.FechaDesde).ToList();

                    return domicilios.Adapt<IList<DomicilioDTO>>();
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DomicilioDTO> PostDomicilio(DomicilioDTO domicilio, string user)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    Domicilio nuevoDomicilio = new Domicilio();
                    nuevoDomicilio.IdUsuario = findUsuario.IdUsuario;
                    nuevoDomicilio.Altura = int.Parse(domicilio.Altura);
                    nuevoDomicilio.Calle = domicilio.Calle;
                    nuevoDomicilio.Departamento = domicilio.Departamento;
                    nuevoDomicilio.CodigoPostal = domicilio.CodigoPostal;
                    nuevoDomicilio.Aclaracion = domicilio.Aclaracion;
                    nuevoDomicilio.FechaDesde = DateTime.Now;
                    nuevoDomicilio.FechaActualizacion = DateTime.Now;

                    await _unitOfWork.DomicilioRepository.Insert(nuevoDomicilio);
                    await _unitOfWork.CommitAsync();

                    return nuevoDomicilio.Adapt<DomicilioDTO>();
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DomicilioDTO> EliminarDomicilio(int idDomicilio, string user)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    Domicilio findDomicilio = (await _unitOfWork.DomicilioRepository.GetByCriteria(x => x.IdDomicilio == idDomicilio && x.IdUsuario == findUsuario.IdUsuario)).FirstOrDefault();

                    if (findDomicilio != null)
                    {
                        findDomicilio.FechaHasta = DateTime.Now;
                        findDomicilio.FechaActualizacion = DateTime.Now;

                        await _unitOfWork.DomicilioRepository.Update(findDomicilio);
                        await _unitOfWork.CommitAsync();

                        return findDomicilio.Adapt<DomicilioDTO>();
                    }
                    else
                    {
                        throw new Exception("No se encontro el domicilio");
                    }
                }
                else
                {
                    throw new Exception("No se encontro el usuario");
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw ex;
            }
        }

        public async Task<DomicilioDTO> GetDomicilio(int idDomicilio, string user)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    Domicilio findDomicilio = (await _unitOfWork.DomicilioRepository.GetByCriteria(x => x.IdDomicilio == idDomicilio && x.IdUsuario == findUsuario.IdUsuario)).FirstOrDefault();

                    if (findDomicilio != null)
                    {
                        return findDomicilio.Adapt<DomicilioDTO>();
                    }
                    else
                    {
                        throw new ApiException("No se encontro el domicilio", (int)HttpStatusCode.NotFound, "No se encontro el domicilio");
                    }
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<DomicilioDTO> EditarDomicilio(DomicilioDTO domicilio, string user)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    Domicilio findDomicilio = (await _unitOfWork.DomicilioRepository.GetByCriteria(x => x.IdDomicilio == domicilio.IdDomicilio && x.IdUsuario == findUsuario.IdUsuario)).FirstOrDefault();

                    if (findDomicilio != null)
                    {
                        findDomicilio.Altura = int.Parse(domicilio.Altura);
                        findDomicilio.Calle = domicilio.Calle;
                        findDomicilio.Departamento = domicilio.Departamento;
                        findDomicilio.CodigoPostal = domicilio.CodigoPostal;
                        findDomicilio.Aclaracion = domicilio.Aclaracion;
                        findDomicilio.FechaActualizacion = DateTime.Now;

                        await _unitOfWork.DomicilioRepository.Update(findDomicilio);
                        await _unitOfWork.CommitAsync();

                        return findDomicilio.Adapt<DomicilioDTO>();
                    }
                    else
                    {
                        throw new ApiException("No se encontro el domicilio", (int)HttpStatusCode.NotFound, "No se encontro el domicilio");
                    }
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PedidoDTO>> GetPedidos(string? user)
        {
            List<Pedido> pedidos = new List<Pedido>();
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    pedidos = (await _unitOfWork.GenericRepository<Pedido>().GetByCriteriaIncludingSpecificRelations(x => x.IdUsuario == findUsuario.IdUsuario && x.FechaBaja == null,
                     query => query.Include(p => p.Pago)
                                    .Include(p => p.Envio)
                                    .ThenInclude(e => e.EstadoEnvio)
                                    .Include(p => p.PublicacionPedido)
                                    .ThenInclude(pp => pp.Publicacion)
                                    .ThenInclude(publi => publi.IdProductoNavigation)
                                    .ThenInclude(prod => prod.IdCategoriaNavigation)
                                    .Include(p => p.PublicacionPedido)
                                    .ThenInclude(pp => pp.Publicacion)
                                    .ThenInclude(publi => publi.IdSucursalNavigation)
                                    .Include(p => p.Envio)
                                    .ThenInclude(e => e.Domicilio)
                     )).ToList().OrderByDescending(x => x.FechaAlta).ToList();


                    return pedidos.Adapt<List<PedidoDTO>>();

                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<byte[]> CrearPDF(int idPedido)
        {
            try
            {


                Pedido findPedido = (await _unitOfWork.GenericRepository<Pedido>().GetByCriteriaIncludingSpecificRelations(x => x.Id == idPedido,
                     query => query.Include(p => p.Pago)
                                    .Include(p => p.Envio)
                                    .ThenInclude(e => e.EstadoEnvio)
                                    .Include(p => p.PublicacionPedido)
                                    .ThenInclude(pp => pp.Publicacion)
                                    .ThenInclude(publi => publi.IdProductoNavigation)
                                    .ThenInclude(prod => prod.IdCategoriaNavigation)
                                    .Include(p => p.PublicacionPedido)
                                    .ThenInclude(pp => pp.Publicacion)
                                    .ThenInclude(publi => publi.IdSucursalNavigation)
                                    .Include(p => p.Envio)
                                    .ThenInclude(e => e.Domicilio)
                                    .Include(p => p.Usuario)
                     )).FirstOrDefault();

                if (findPedido == null)
                {
                    throw new ApiException("No se encontro el pedido", 404, "No se encontro el pedido");
                }

                PedidoDTO pedido = findPedido.Adapt<PedidoDTO>();

                //creo el pdf
                return await _serviceReporte.CrearPDF(pedido);
            }
            catch (ApiException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal async Task<Usuario> GetEmailUsuario(int idUsuario)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.IdUsuario == idUsuario)).FirstOrDefault();

                if (findUsuario != null)
                {
                    return findUsuario;
                }
                else
                {
                    throw new ApiException("No se encontro el usuario", (int)HttpStatusCode.NotFound, "No se encontro el usuario");
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


