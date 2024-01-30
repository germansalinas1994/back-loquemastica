using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using MercadoPago.Resource.User;
using AutoWrapper.Wrappers;
using System.Net;
using System.Globalization;

namespace BussinessLogic.Services
{
    public class ServiceSucursal
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceReporte _serviceReporte;


        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceSucursal(IUnitOfWork unitOfWork, ServiceReporte serviceReporte)
        {
            _unitOfWork = unitOfWork;
            _serviceReporte = serviceReporte;

        }



        public async Task<List<SucursalDTO>> GetSucursales()
        {
            try
            {
                IList<Sucursal> sucursales = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.FechaBaja == null)).OrderBy(x => x.IdSucursal).ToList();
                return sucursales.Adapt<List<SucursalDTO>>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task EliminarSucursal(int id)
        {
            try
            {
                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);
                if (sucursal == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                sucursal.FechaBaja = DateTime.Now;
                sucursal.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Update(sucursal);
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

        public async Task<SucursalDTO> GetSucursalEmail(string user)
        {

            try
            {
                Sucursal sucursal = _unitOfWork.GenericRepository<Sucursal>().GetAll().Result.Where(s => s.EmailSucursal == user).FirstOrDefault();
                return sucursal.Adapt<SucursalDTO>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<List<PedidoSucursalDTO>> GetPedidosSucursal(string user)
        {
            try
            {
                //voy a filtrar por mes y año actual
                int mesActual = DateTime.Now.Month;
                int anioActual = DateTime.Now.Year;

                List<PedidoSucursalDTO> pedidosDTO = new List<PedidoSucursalDTO>();
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == user)).FirstOrDefault();
                List<Pedido> pedidos = (await _unitOfWork.GenericRepository<Pedido>().GetByCriteriaIncludingSpecificRelations(x => x.IdSucursalPedido == sucursal.IdSucursal && x.FechaAlta.Month == mesActual && x.FechaAlta.Year == anioActual,
                    query => query.Include(p => p.PublicacionPedido)
                    .ThenInclude(pu => pu.Publicacion)
                    .ThenInclude(pr => pr.IdProductoNavigation)
                    .Include(e => e.Envio)
                    .ThenInclude(ee => ee.EstadoEnvio)
                    .Include(u => u.Usuario)
                    )).ToList();
                if (pedidos != null && pedidos.Count > 0)
                {
                    pedidosDTO = pedidos.Select(p => new PedidoSucursalDTO
                    {
                        Id = p.Id,
                        Fecha = p.FechaAlta,
                        Orden_MercadoPago = p.Orden_MercadoPago,
                        EstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.Descripcion : "Retira en la Sucursal",
                        EmailUsuario = p.Usuario.Email,
                        idEstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.IdEstadoEnvio : null,
                        Total = p.Total,
                        DetallePedido = p.PublicacionPedido.Select(pp => new DetallePedidoDTO
                        {
                            // Aquí mapea los campos del detalle del pedido según tus necesidades
                            Id = pp.IdPublicacion,
                            Cantidad = pp.Cantidad,
                            Precio = pp.Precio,
                            NombreProducto = pp.Publicacion.IdProductoNavigation.Nombre,
                            SubTotal = pp.Cantidad * pp.Precio
                            // Agrega más campos según sea necesario
                        }).ToList()
                    }).ToList();


                }
                return pedidosDTO.OrderByDescending(p => p.Fecha).ToList();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> GetCantidadPedidosPorSucursal(int id, int mes, int anio)
        {
            try
            {
                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);

                if (sucursal == null)
                {
                    throw new Exception($"The sucursal with ID {id} does not exist");
                }

                int cantidadPedidos = (await _unitOfWork.GenericRepository<Pedido>()
                    .GetByCriteriaIncludingSpecificRelations(
                        x => x.IdSucursalPedido == sucursal.IdSucursal &&
                             x.FechaAlta.Month == mes &&
                             x.FechaAlta.Year == anio))
                    .Count();

                return cantidadPedidos;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<SucursalDTO> GetSucursalById(int id)
        {

            try
            {
                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);
                if (sucursal == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                return sucursal.Adapt<SucursalDTO>();
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

        // Inside ServiceSucursal
        public async Task<int> GetCantidadPedidosPorSucursal(int sucursalId)
        {
            try
            {
                // Get current month and year
                DateTime currentDate = DateTime.Now;
                int currentMonth = currentDate.Month;
                int currentYear = currentDate.Year;
                int cantidadPedidos = await GetCantidadPedidosPorSucursal(sucursalId, currentMonth, currentYear);
                // Call your existing method to get the quantity of orders for the specified sucursal in the current month
                return cantidadPedidos;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while retrieving order count", ex);
            }
        }


        public async Task CargarSucursal(SucursalDTO sucursal)
        {
            try
            {

                Sucursal sucursalExistente = _unitOfWork.GenericRepository<Sucursal>().GetAll().Result.Where(s => s.EmailSucursal == sucursal.EmailSucursal).FirstOrDefault();
                if (sucursalExistente != null)
                {
                    throw new ApiException("El mail de la sucursal ya existe");
                }

                await _unitOfWork.BeginTransactionAsync();


                Sucursal sucursalBase = sucursal.Adapt<Sucursal>();
                sucursalBase.EmailSucursal = sucursal.EmailSucursal;
                sucursalBase.Nombre = sucursal.Nombre;
                sucursalBase.Direccion = sucursal.Direccion;
                sucursalBase.FechaAlta = DateTime.Now;
                sucursalBase.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Insert(sucursalBase);

                await _unitOfWork.CommitAsync();
                IList<Producto> productos = await _unitOfWork.GenericRepository<Producto>().GetByCriteria(x => x.FechaBaja == null);
                foreach (var producto in productos)
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.IdProducto = producto.IdProducto;
                    publicacion.IdSucursal = sucursalBase.IdSucursal;
                    publicacion.Stock = (sucursalBase.IdSucursal == SucursalDTO.IdsucursalGenerica) ? 1 : 0; // Asigna 1 solo si es la sucursal con Id 1
                    publicacion.FechaDesde = DateTime.Now;
                    publicacion.FechaActualizacion = DateTime.Now;
                    await _unitOfWork.GenericRepository<Publicacion>().Insert(publicacion);
                }
            }
            catch (ApiException)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw new ApiException(e);
            }
        }

        public async Task EditarSucursal(SucursalDTO sucursal)
        {
            try
            {
                Sucursal sucursalExistente = await _unitOfWork.GenericRepository<Sucursal>().GetById(sucursal.IdSucursal);
                if (sucursalExistente == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                sucursalExistente.Nombre = sucursal.Nombre;
                sucursalExistente.Direccion = sucursal.Direccion;
                sucursalExistente.EmailSucursal = sucursal.EmailSucursal;
                sucursalExistente.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Update(sucursalExistente);
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

        public async Task CambiarEstadoEnvio(int idPedido, int idEstadoEnvio)
        {
            try
            {
                Envio envio = (await _unitOfWork.GenericRepository<Envio>().GetByCriteria(x => x.IdPedido == idPedido)).FirstOrDefault();
                if (envio == null)
                {
                    throw new ApiException("El pedido no tiene envio", (int)HttpStatusCode.BadRequest, "El pedido no tiene envio");
                }
                Estadoenvio estadonEnvio = await _unitOfWork.GenericRepository<Estadoenvio>().GetById(idEstadoEnvio);
                if (estadonEnvio == null)
                {
                    throw new ApiException("El estado del pedido no existe", (int)HttpStatusCode.BadRequest, "El estado del pedido no existe");
                }

                envio.IdEstadoEnvio = idEstadoEnvio;
                envio.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Envio>().Update(envio);
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

        public async Task<List<PedidoSucursalDTO>> FiltrarPedidosSucursal(int mes, int anio, int estado, string user)
        {

            try
            {
                //primero obtengo la sucursal del usuario
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == user)).FirstOrDefault();
                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                return await GetPedidosSucursal(mes, anio, estado, sucursal.IdSucursal);


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

        public async Task<byte[]> GenerarReportePedidosSucursal(int mes, int anio, string user)
        {
            try
            {
                DatosReporteDTO datosReporte = await GetPedidosMensuales(mes, anio, user);

                byte[] reporte = await _serviceReporte.GenerarReportePedidosSucursal(datosReporte);

                return reporte;

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


        public async Task<DatosReporteDTO> GetPedidosMensuales(int mes, int anio, string user)
        {

            try
            {

                DatosReporteDTO datosReporte = new DatosReporteDTO();

                datosReporte.MesReporte = mes;
                datosReporte.AnioReporte = anio;



                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == user)).FirstOrDefault();
                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                datosReporte.NombreSucursal = sucursal.Nombre;
                datosReporte.DireccionSucursal = sucursal.Direccion;


                return await GetDatosReporte(mes, anio, datosReporte, sucursal.IdSucursal);

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

        private async Task<DatosReporteDTO> GetDatosReporte(int mes, int anio, DatosReporteDTO datosReporte, int idSucursal)
        {
            List<PedidosReporteDTO> pedidosDTO = new List<PedidosReporteDTO>();

            var query = (await _unitOfWork.GenericRepository<Pedido>().Search()).Where(x => x.IdSucursalPedido == idSucursal && x.FechaAlta.Month == mes && x.FechaAlta.Year == anio);
            //hago la consulta, con el search que arme 
            List<Pedido> pedidos = await query.Include(p => p.PublicacionPedido)
                                                   .ThenInclude(pu => pu.Publicacion)
                                                   .ThenInclude(pr => pr.IdProductoNavigation)
                                                   .Include(e => e.Envio)
                                                   .ThenInclude(ee => ee.EstadoEnvio)
                                                   .Include(u => u.Usuario)
                                                   .ToListAsync();
            if (pedidos != null && pedidos.Count > 0)
            {
                pedidosDTO = pedidos.Select(p => new PedidosReporteDTO
                {
                    FechaPedido = p.FechaAlta,
                    Orden_MercadoPago = p.Orden_MercadoPago,
                    EstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.Descripcion : "Retira en la Sucursal",
                    EmailUsuario = p.Usuario.Email,
                    Total = p.Total,

                }).ToList();
            }

            datosReporte.Pedidos = pedidosDTO.OrderByDescending(p => p.FechaPedido).ToList();

            return datosReporte;
        }

        public async Task<List<PedidoSucursalDTO>> FiltrarPedidosAdministrador(int mes, int anio, int sucursalSeleccionada, int estado)
        {
            try
            {
                //primero obtengo la sucursal del usuario
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetById(sucursalSeleccionada));
                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                return await GetPedidosSucursal(mes, anio, estado, sucursal.IdSucursal);

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

        private async Task<List<PedidoSucursalDTO>> GetPedidosSucursal(int mes, int anio, int estado, int idSucursal)
        {
            //creo el objeto que voy a devolver
            List<PedidoSucursalDTO> pedidosDTO = new List<PedidoSucursalDTO>();

            //si el estado es 0, es porque no se selecciono ningun estado, entonces traigo todos los pedidos de la sucursal y solo filtro por mes y año
            //si el estado es 4 es por que es sin envio, entonces deberia traer todos los pedidos que no tengan envio
            // Inicializa la consulta base
            var query = (await _unitOfWork.GenericRepository<Pedido>().Search()).Where(x => x.IdSucursalPedido == idSucursal && x.FechaAlta.Month == mes && x.FechaAlta.Year == anio);
            if (estado != EnvioDTO.EstadoEnvioTodos)
            {
                if (estado == EnvioDTO.EstadoEnvioSinEnvio)
                {
                    query = query.Where(x => x.Envio == null);
                }
                else
                {
                    query = query.Where(x => x.Envio.IdEstadoEnvio == estado);
                }
            }

            //hago la consulta, con el search que arme 
            List<Pedido> pedidos = await query.Include(p => p.PublicacionPedido)
                                                   .ThenInclude(pu => pu.Publicacion)
                                                   .ThenInclude(pr => pr.IdProductoNavigation)
                                                   .Include(e => e.Envio)
                                                   .ThenInclude(ee => ee.EstadoEnvio)
                                                   .Include(u => u.Usuario)
                                                   .ToListAsync();
            if (pedidos != null && pedidos.Count > 0)
            {
                pedidosDTO = pedidos.Select(p => new PedidoSucursalDTO
                {
                    Id = p.Id,
                    Fecha = p.FechaAlta,
                    Orden_MercadoPago = p.Orden_MercadoPago,
                    EstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.Descripcion : "Retira en la Sucursal",
                    EmailUsuario = p.Usuario.Email,
                    idEstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.IdEstadoEnvio : null,
                    Total = p.Total,
                    DetallePedido = p.PublicacionPedido.Select(pp => new DetallePedidoDTO
                    {
                        // Aquí mapea los campos del detalle del pedido según tus necesidades
                        Id = pp.IdPublicacion,
                        Cantidad = pp.Cantidad,
                        Precio = pp.Precio,
                        NombreProducto = pp.Publicacion.IdProductoNavigation.Nombre,
                        SubTotal = pp.Cantidad * pp.Precio
                        // Agrega más campos según sea necesario
                    }).ToList()
                }).ToList();
            }
            return pedidosDTO.OrderByDescending(p => p.Fecha).ToList();
        }

        public async Task<byte[]> GenerarReportePedidosAdministrador(int mes, int anio, int sucursalSeleccionada)
        {
            try
            {
                DatosReporteDTO datosReporte = await GetPedidosMensualesAdministrador(mes, anio, sucursalSeleccionada);

                byte[] reporte = await _serviceReporte.GenerarReportePedidosSucursal(datosReporte);

                return reporte;

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
        public async Task<DatosReporteDTO> GetPedidosMensualesAdministrador(int mes, int anio, int idSucursal)
        {

            try
            {

                DatosReporteDTO datosReporte = new DatosReporteDTO();

                datosReporte.MesReporte = mes;
                datosReporte.AnioReporte = anio;



                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(idSucursal);
                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                datosReporte.NombreSucursal = sucursal.Nombre;
                datosReporte.DireccionSucursal = sucursal.Direccion;


                return await GetDatosReporte(mes, anio, datosReporte, sucursal.IdSucursal);

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



        // public async Task<Dictionary<string, int>> GetPedidosSucursalUltimosMeses(string userEmail)

        // {

        //     try
        //     {

        //         var fechaInicio = DateTime.Now.AddMonths(-6);
        //         Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == userEmail)).FirstOrDefault();

        //         if (sucursal == null)
        //         {
        //             throw new ApiException("No se encontró la sucursal");
        //         }

        //         var pedidos = (await _unitOfWork.GenericRepository<Pedido>()
        //                                         .GetByCriteria(p => p.IdSucursalPedido == sucursal.IdSucursal &&
        //                                                             p.FechaAlta >= fechaInicio))
        //                                         .ToList();

        //         var pedidosPorMes = pedidos.GroupBy(p => new { p.FechaAlta.Year, p.FechaAlta.Month }).OrderByDescending(g => g.Key.Year).ThenBy(g => g.Key.Month)
        //                                    .Select(async group => new
        //                                    {
        //                                        //    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month),
        //                                        Mes = await GetNombreMes(group.Key.Month),
        //                                        Cantidad = group.Count()
        //                                    })
        //                                    .ToDictionary(g => g.Result.Mes, g => g.Result.Cantidad);

        //         return pedidosPorMes;


        //     }
        //     catch (ApiException)
        //     {
        //         throw;
        //     }
        //     catch (Exception e)
        //     {
        //         throw new ApiException(e);
        //     }


        // }

        public async Task<Dictionary<string, int>> GetPedidosSucursalUltimosMeses(string userEmail)
        {
            try
            {
                var fechaInicio = DateTime.Now.AddMonths(-6);
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == userEmail)).FirstOrDefault();

                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                var pedidos = (await _unitOfWork.GenericRepository<Pedido>()
                                                .GetByCriteria(p => p.IdSucursalPedido == sucursal.IdSucursal && p.FechaAlta >= fechaInicio))
                                                .ToList();

                //agrupo los pedidos por mes y año, creo un diccionario con la clave el mes y el valor la cantidad de pedidos, el 1 es el dia que le pongo para que no me tire error
                var pedidosPorMes = pedidos.GroupBy(p => new { p.FechaAlta.Year, p.FechaAlta.Month })
                                           .ToDictionary(group => new DateTime(group.Key.Year, group.Key.Month, 1), group => group.Count());

                //creo una lista de los ultimos 6 meses, en formato yyyy-MM
                var rangoMeses = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(-i).ToString("yyyy-MM")).ToList();

                // Para cada mes en el rango de los últimos 6 meses, se verifica si hay pedidos en ese mes. Si es así, se usa el conteo de pedidos; si no, se asigna 0. Esto asegura que todos los meses en el rango estén representados en el resultado final.
                var resultado = rangoMeses.ToDictionary(
                    mes => mes,
                    mes => pedidosPorMes.ContainsKey(DateTime.ParseExact(mes + "-01", "yyyy-MM-dd", null)) ? pedidosPorMes[DateTime.ParseExact(mes + "-01", "yyyy-MM-dd", null)] : 0
                );

                // Pongo los meses con nombre
                var resultadoFinal = new Dictionary<string, int>();
                foreach (var item in resultado)
                {
                    var mesNombre = await GetNombreMes(DateTime.ParseExact(item.Key + "-01", "yyyy-MM-dd", null).Month);
                    resultadoFinal[mesNombre] = item.Value;
                }

                return resultadoFinal;
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


        public async Task<string> GetNombreMes(int fecha)
        {
            try
            {
                //segun el numero de mes devuelvo el nombre
                switch (fecha)
                {
                    case 1:
                        return "Enero";
                    case 2:
                        return "Febrero";
                    case 3:
                        return "Marzo";
                    case 4:
                        return "Abril";
                    case 5:
                        return "Mayo";
                    case 6:
                        return "Junio";
                    case 7:
                        return "Julio";
                    case 8:
                        return "Agosto";
                    case 9:
                        return "Septiembre";
                    case 10:
                        return "Octubre";
                    case 11:
                        return "Noviembre";
                    case 12:
                        return "Diciembre";
                    default:
                        return "Error";
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}