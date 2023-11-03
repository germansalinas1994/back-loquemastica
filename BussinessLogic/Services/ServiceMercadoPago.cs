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

        //inyecto el settings por el constructor, para poder usar las credenciales de mercado pago
        public ServiceMercadoPago(IOptions<MercadoPagoDevSettings> mercadoPagoSettingsOptions)
        {
            _mercadoPagoSettings = mercadoPagoSettingsOptions.Value;
        }


        //implementacion mercado pago 

        public async Task<string> GetPreferenceMP(List<PublicacionDTO> publicaciones)
        {


            //las credenciales que puse son de prueba, hay que cambiarlas por las de produccion, estas las cree en la cuenta del vendedor de prueba
            MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

            // Crea el objeto de request de la preference


            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>(),
                Purpose = "wallet_purchase",


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


        public async Task<string> GetPaymentInfo(string paymentId)
        {
            MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

            // Crear el cliente de pago
            var client = new PaymentClient();

            // Obtener la información de pago
            Payment payment = await client.GetAsync(long.Parse(paymentId));

            long? idPago = payment.Id;
            string status = payment.Status;
            string statusDetail = payment.StatusDetail;
            PaymentOrder paymentOrder = payment.Order;
            

            
            return "funciono";
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