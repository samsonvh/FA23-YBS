using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Storage;
// using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace YBS.Service.Services.Implements
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private string BucketName;
        private readonly IConfiguration _configuration;
        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            BucketName = _configuration["Firebase:BucketName"];
        }
        public async Task<Uri> UploadFile(string name, IFormFile image, int counter, string objectType, string type = null)
        {
            var stream = image.OpenReadStream();
            string task;
        
            switch (objectType)
            {
                case "Yacht":
                    task = await new FirebaseStorage(BucketName)
                           .Child(objectType)
                           .Child(type)
                           .Child(name)
                           .Child(image.FileName)
                           .PutAsync(stream);
                    break;
                default:
                    task = await new FirebaseStorage(BucketName)
                            .Child(objectType)
                            .Child(name)
                            .Child(image.FileName)
                            .PutAsync(stream);
                    break;
            }

            var imageUrl = task;
            var imageUri = new Uri(imageUrl);
            return imageUri;
        }
    }
}