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
        private readonly IUnitOfWork _unitOfWork;

        //inyecto el settings por el constructor, para poder usar las credenciales de mercado pago
        public ServiceMercadoPago(IOptions<MercadoPagoDevSettings> mercadoPagoSettingsOptions, IUnitOfWork unitOfWork)
        {
            _mercadoPagoSettings = mercadoPagoSettingsOptions.Value;
            _unitOfWork = unitOfWork;
        }


        //implementacion mercado pago 

        public async Task<string> GetPreferenceMP(List<PublicacionDTO> publicaciones, int idUsuario)
        {


            //las credenciales que puse son de prueba, hay que cambiarlas por las de produccion, estas las cree en la cuenta del vendedor de prueba
            MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

            // Crea el objeto de request de la preference


            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>(),
                Purpose = "wallet_purchase",
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddDays(1),



            };

            request.Metadata = new Dictionary<string, object>
            {
                {"idUsuario", idUsuario.ToString() }
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
                    UnitPrice = (decimal?)publicacion.Precio,  // Debes proporcionar el precio de la publicación aquí
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

                //recupero todos los id de las publicaciones con su cantidad
                List<PublicacionDTO> publicaciones = new List<PublicacionDTO>();

                foreach (var item in payment.AdditionalInfo.Items)
                {
                    PublicacionDTO publicacion = new PublicacionDTO();
                    publicacion.IdPublicacion = Convert.ToInt32(item.Id);
                    publicacion.Cantidad = item.Quantity.Value;
                    publicaciones.Add(publicacion);
                }

                //recupero el total del pago
                decimal total = payment.TransactionAmount.Value;

                //abro una transaccion



                await _unitOfWork.BeginTransactionAsync();

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
                        Publicacion publicacionBD = await _unitOfWork.GenericRepository<Publicacion>().GetById(publicacion.IdPublicacion);

                        publicacionBD.Stock = publicacionBD.Stock - publicacion.Cantidad;

                        await _unitOfWork.GenericRepository<Publicacion>().Update(publicacionBD);
                    }

                    //guardo los cambios

                    await _unitOfWork.CommitAsync();


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



    }
}