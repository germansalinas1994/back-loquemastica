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
// Agrega credenciales

namespace BussinessLogic.Services
{
    public class ServiceMercadoPago
    {
        //Instancio el UnitOfWork que vamos a usar

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceMercadoPago()
        {
        }


        //implementacion mercado pago 

        public async Task<string> GetPreferenceMP(List<PublicacionDTO> publicaciones)
        {
            MercadoPagoConfig.AccessToken = "TEST-777128697650019-101712-6864c880dae0b60469ef6bfe3004e124-178444398";
            // Crea el objeto de request de la preference


         var request = new PreferenceRequest
     {
        Items = new List<PreferenceItemRequest>()
    };

            // Itera sobre las publicaciones y crea un ítem para cada una
            foreach (var publicacion in publicaciones)
            {
                var item = new PreferenceItemRequest
                {
                    Title = publicacion.IdProductoNavigation.Nombre,    // Debes proporcionar el título de la publicación aquí
                    Quantity = publicacion.Cantidad,
                    CurrencyId = "ARS",
                    UnitPrice =  (decimal?)publicacion.Precio,  // Debes proporcionar el precio de la publicación aquí
                };

                request.Items.Add(item);
            }
            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference.Id;

        }

    }
}