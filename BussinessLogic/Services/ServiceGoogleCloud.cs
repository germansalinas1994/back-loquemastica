
using BussinessLogic.DTO;

using AutoWrapper.Wrappers;
using Microsoft.Extensions.Options;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
// Agrega credenciales

namespace BussinessLogic.Services
{
    public class ServiceGoogleCloud
    {


        //instancio el settings para poder usar las credenciales de mercado pago
        private readonly GoogleCloudStorage _googleCloudStorageSettings;
        private readonly StorageClient _storageClient;
        private readonly GoogleCredential _googleCredentials;


        //inyecto el settings por el constructor, para poder usar las credenciales de mercado pago
        public ServiceGoogleCloud(IOptions<GoogleCloudStorage> googleCloudStorageSettingsOptions)
        {
            _googleCloudStorageSettings = googleCloudStorageSettingsOptions.Value;

            _googleCredentials = GoogleCredential.FromFile(_googleCloudStorageSettings.CredentialsFilePath);

        }


        public async Task<string> SubirImagenAsync(IFormFile archivo)
        {
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);

                using (var storageClient = StorageClient.Create(_googleCredentials))
                {
                    var objectName = archivo.FileName;
                    var uploadFile = await storageClient.UploadObjectAsync(
                         bucket: _googleCloudStorageSettings.BucketName,
                         objectName: objectName,
                         contentType: archivo.ContentType,
                         source: memoryStream,
                         options: new UploadObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }
                     );

                    // Construir la URL pública para visualización
                    var publicUrl = $"https://storage.googleapis.com/{_googleCloudStorageSettings.BucketName}/{objectName}";
                    return publicUrl;
                }
            }
        }



    }
}