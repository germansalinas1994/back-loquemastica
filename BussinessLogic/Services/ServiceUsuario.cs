using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;



namespace BussinessLogic.Services
{
    public class ServiceUsuario
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceUsuario(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<UsuarioDTO> CargarUsuarioAuth0(RequestCargarUsuarioDTO usuario)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == usuario.Email)).FirstOrDefault();

                //si encuentra el usuario es que ya esta cargado sino lo cargo

                if (findUsuario != null)
                {
                    return findUsuario.Adapt<UsuarioDTO>();
                }
                else
                {
                    Usuario nuevoUsuario = new Usuario();
                    nuevoUsuario.Email = usuario.Email;
                    nuevoUsuario.FechaAlta = DateTime.Now;
                    nuevoUsuario.FechaModificacion = DateTime.Now;

                    await _unitOfWork.UsuarioRepository.Insert(nuevoUsuario);
                    await _unitOfWork.CommitAsync();

                    return nuevoUsuario.Adapt<UsuarioDTO>();


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
                    return null;
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
                    return null;
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

        public async Task<IList<DomicilioDTO>> GetDomicilios(string email){


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

        public async Task<DomicilioDTO> PostDomicilio(DomicilioDTO domicilio, string user)
        {
            try
            {
                Usuario findUsuario = (await _unitOfWork.UsuarioRepository.GetByCriteria(x => x.Email == user)).FirstOrDefault();

                if (findUsuario != null)
                {
                    Domicilio nuevoDomicilio = new Domicilio();
                    nuevoDomicilio.IdUsuario = findUsuario.IdUsuario;
                    nuevoDomicilio.Altura = domicilio.Altura;
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
    }
}