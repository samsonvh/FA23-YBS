using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Storage;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace YBS.Service.Services.Implements
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private const string BucketName = "yacht-booking-system-3bc15.appspot.com";
        public async Task<Uri> UploadFile(string name, IFormFile image, int counter, string objectType,string type)
        {
            var stream = image.OpenReadStream();
            var task = await new FirebaseStorage(BucketName)
                            .Child(objectType)
                            .Child(type)
                            .Child(name)
                            .Child(image.FileName)
                            .PutAsync(stream);
            var imageUrl =  task;
            var imageUri = new Uri(imageUrl);
            return imageUri;
        }
    }
}