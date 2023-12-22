using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;



// SDK de Mercado Pago
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Options;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using MercadoPago.Client;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Resource.MerchantOrder;
// Agrega credenciales

namespace BussinessLogic.Services
{
    public class ServiceMercadoPago
    {


        //instancio el settings para poder usar las credenciales de mercado pago
        private readonly MercadoPagoDevSettings _mercadoPagoSettings;
        private ServiceUsuario _serviceUsuario;

        private readonly IUnitOfWork _unitOfWork;

        //inyecto el settings por el constructor, para poder usar las credenciales de mercado pago
        public ServiceMercadoPago(IOptions<MercadoPagoDevSettings> mercadoPagoSettingsOptions, IUnitOfWork unitOfWork, ServiceUsuario serviceUsuario)
        {
            _mercadoPagoSettings = mercadoPagoSettingsOptions.Value;
            _unitOfWork = unitOfWork;
            _serviceUsuario = serviceUsuario;
        }


        //implementacion mercado pago 

        public async Task<string> GetPreferenceMP(List<PublicacionDTO> publicaciones, int idUsuario, int idDomicilio)
        {


            //las credenciales que puse son de prueba, hay que cambiarlas por las de produccion, estas las cree en la cuenta del vendedor de prueba
            MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

            // Crea el objeto de request de la preference

            //si el id del domicilio es 0, es porque es un retiro en sucursal, entonces no tengo que agregar el domicilio


            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>(),
                Purpose = "wallet_purchase",
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddDays(1),

            };

            if (idDomicilio != 0)
            {
                Domicilio domicilio = await _unitOfWork.GenericRepository<Domicilio>().GetById(idDomicilio);

                if (domicilio != null)
                {

                    //agrego el domicilio a la preferencia
                    request.Shipments = new PreferenceShipmentsRequest
                    {
                        ReceiverAddress = new PreferenceReceiverAddressRequest
                        {
                            StreetName = domicilio.IdDomicilio.ToString(),

                        }
                    };

                }
            }

            request.Metadata = new Dictionary<string, object>
                {
                    {"idUsuario", idUsuario.ToString() },
                    {"idDomicilio", idDomicilio.ToString() }
                };




            request.BackUrls = new PreferenceBackUrlsRequest
            {
                Success = _mercadoPagoSettings.SuccessUrl,
                Failure = _mercadoPagoSettings.FailureUrl,
                Pending = _mercadoPagoSettings.PendingUrl
            };
            request.NotificationUrl = _mercadoPagoSettings.NotificationUrl;

            request.AutoReturn = "approved";

            // Itera sobre las publicaciones y crea un ítem para cada una
            foreach (var publicacion in publicaciones)
            {
                var item = new PreferenceItemRequest
                {
                    Id = publicacion.IdPublicacion.ToString(),
                    Description = publicacion.IdProductoNavigation.Descripcion,
                    PictureUrl = publicacion.IdProductoNavigation.UrlImagen,
                    Title = publicacion.IdProductoNavigation.Nombre,    // Debes proporcionar el título de la publicación aquí
                    Quantity = publicacion.Cantidad,
                    CurrencyId = "ARS",
                    UnitPrice = (decimal?)publicacion.IdProductoNavigation.Precio,  // Debes proporcionar el precio de la publicación aquí
                };



                request.Items.Add(item);
            }
            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference.InitPoint;

        }


        public async Task CrearPedido(string paymentId)
        {
            MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

            // Crear el cliente de pago
            var client = new PaymentClient();

            // Obtener la información de pago
            Payment payment = await client.GetAsync(long.Parse(paymentId));
            await _unitOfWork.BeginTransactionAsync();


            //si existe el pago y el status es aprobado, tengo que crear el pago en la base de datos
            if (payment != null && payment.Status == "approved" && payment.StatusDetail == "accredited")
            {


                //Busco que no exista el pedido ni el pago en la base de datos
                Pago pagoDB = (await _unitOfWork.GenericRepository<Pago>().GetByCriteria(x => x.IdPagoMercadoPago == payment.Id.Value)).FirstOrDefault();

                if (pagoDB != null)
                {
                    //devuelvo ok para avisar a mercadopago que ya todo se ejecuto correctamente
                    return;
                }

                //el id del pago es el id lo uso para guardarlo en el pago
                long idPago = payment.Id.Value;

                //tengo que recuperar el id usuario de la metadata
                //el id usuario lo uso para guardarlo en el pedido
                int idUsuario = Convert.ToInt32(payment.Metadata["id_usuario"].ToString());

                //el id de la orden de pago lo uso para guardarlo en el pedido
                long ordenDeCompra = payment.Order.Id.Value;

                int idDomicilio = Convert.ToInt32(payment.Metadata["id_domicilio"].ToString());

                //si el id del domicilio es 0, es porque es un retiro en sucursal, entonces no tengo que agregar el domicilio





                //recupero todos los id de las publicaciones con su cantidad
                List<SearchPublicacionCarritoDTO> publicaciones = new List<SearchPublicacionCarritoDTO>();

                foreach (var item in payment.AdditionalInfo.Items)
                {
                    SearchPublicacionCarritoDTO publicacion = new SearchPublicacionCarritoDTO();
                    publicacion.Id = Convert.ToInt32(item.Id);
                    publicacion.Cantidad = item.Quantity.Value;
                    publicaciones.Add(publicacion);
                }

                //recupero el total del pago
                decimal total = payment.TransactionAmount.Value;

                //abro una transaccion




                try
                {
                    //creo el pedido

                    Pedido pedido = new Pedido();
                    pedido.IdUsuario = idUsuario;
                    pedido.FechaAlta = DateTime.Now;
                    pedido.FechaModificacion = DateTime.Now;
                    pedido.Orden_MercadoPago = ordenDeCompra;
                    pedido.Total = total;

                    pedido = await _unitOfWork.GenericRepository<Pedido>().Insert(pedido);

                    if (idDomicilio != null && idDomicilio != 0)
                    {
                        Domicilio domicilio = await _unitOfWork.GenericRepository<Domicilio>().GetById(idDomicilio);

                        if (domicilio != null)
                        {
                            //creo el envio

                            Envio envio = new Envio();
                            envio.IdPedido = pedido.Id;
                            envio.FechaAlta = DateTime.Now;
                            envio.FechaModificacion = DateTime.Now;
                            envio.IdEstadoEnvio = Estadoenvio.Ingresado;
                            envio.IdDomicilio = domicilio.IdDomicilio;

                            await _unitOfWork.GenericRepository<Envio>().Insert(envio);
                        }


                    }


                    //creo el pago

                    Pago pago = new Pago();
                    pago.IdPedido = pedido.Id;
                    pago.FechaAlta = DateTime.Now;
                    pago.FechaModificacion = DateTime.Now;
                    pago.EstadoPago = "Aprobado";
                    pago.IdPagoMercadoPago = idPago;
                    pago.Total = total;

                    pago = await _unitOfWork.GenericRepository<Pago>().Insert(pago);

                    //actualizo el stock de las publicaciones

                    foreach (var publicacion in publicaciones)
                    {
                        Publicacion publicacionBD = (await _unitOfWork.GenericRepository<Publicacion>().GetByCriteriaIncludingSpecificRelations(x => x.IdPublicacion == publicacion.Id,
                        query => query.Include(x => x.IdProductoNavigation))).FirstOrDefault();

                        publicacionBD.Stock = publicacionBD.Stock - publicacion.Cantidad;

                        await _unitOfWork.GenericRepository<Publicacion>().Update(publicacionBD);

                        //genero una row en la tabla publicacion_pedido

                        PublicacionPedido publicacionPedido = new PublicacionPedido();
                        publicacionPedido.IdPedido = pedido.Id;
                        publicacionPedido.IdPublicacion = publicacionBD.IdPublicacion;
                        publicacionPedido.Cantidad = publicacion.Cantidad;
                        publicacionPedido.Precio = (decimal)publicacionBD.IdProductoNavigation.Precio.Value;
                        publicacionPedido.FechaAlta = DateTime.Now;
                        publicacionPedido.FechaModificacion = DateTime.Now;

                        await _unitOfWork.GenericRepository<PublicacionPedido>().Insert(publicacionPedido);
                    }

                    //guardo los cambios

                    await _unitOfWork.CommitAsync();



                    // var facturaPdfBytes = await _serviceUsuario.CrearPDF(pedido.Id); // Asumiendo que esto devuelve el PDF como un array de bytes
                    


                }

                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
            }
            else
            {
                //si no existe el pago o el status no es aprobado, tengo que devolver un error
                throw new Exception("Error en el pago");
            }
        }

        public async Task<string> GetMerchantOrder(string urlMercadopago)
        {
            string authorization = $"Bearer {_mercadoPagoSettings.AccessToken}";

            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, urlMercadopago);
                    request.Headers.Add("Authorization", authorization);

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                        return responseBody;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Error";
            }

        }

        public async Task<PedidoDTO> GetOrderMercadoPago(string merchantOrderId, string paymentId)
        {
            //busco el pago en la base de datos por el id del pago
            long idPago = long.Parse(paymentId);
            long idOrden = long.Parse(merchantOrderId);

            Pedido pedido = (await _unitOfWork.GenericRepository<Pedido>().GetByCriteria(x => x.Orden_MercadoPago == idOrden)).FirstOrDefault();

            if (pedido == null)
            {
                throw new Exception("No se encontro el pedido");
            }

            Pago pago = (await _unitOfWork.GenericRepository<Pago>().GetByCriteria(x => x.IdPagoMercadoPago == idPago)).FirstOrDefault();

            if (pago == null)
            {
                throw new Exception("No se encontro el pago");
            }

            return pedido.Adapt<PedidoDTO>();


        }


    }
}