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
        private string PrefixUrl;
        private readonly IConfiguration _configuration;
        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            BucketName = _configuration["Firebase:BucketName"];
            PrefixUrl = _configuration["Firebase:PrefixUrl"];
        }
        public async Task<Uri> UploadFile(string name, IFormFile image, string objectType, string type = null)
        {
            var firebaseStorage = new FirebaseStorage(BucketName);
            var stream = image.OpenReadStream();
            string task;
        
            switch (objectType)
            {
                case "Yacht":
                    task = await firebaseStorage
                           .Child(objectType)
                           .Child(type)
                           .Child(name)
                           .Child(image.FileName)
                           .PutAsync(stream);
                    break;
                default:
                    task = await firebaseStorage
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

        public async Task<Uri> RenamedFolder(string oldName,string oldFileName ,string oldObjectType, string name, IFormFile image, string objectType, string oldType = null,  string type = null)
        {
            var firebaseStorage = new FirebaseStorage(BucketName);
            //get all item exist in old folder
            var stream = image.OpenReadStream();
            var taskFirebase = firebaseStorage.Child(oldObjectType)
                                .Child(oldName)
                                .Child(oldFileName);
            await taskFirebase.DeleteAsync();
            string task;
            switch (objectType)
            {
                case "Yacht":
                    task = await firebaseStorage
                           .Child(objectType)
                           .Child(type)
                           .Child(name)
                           .Child(image.FileName)
                           .PutAsync(stream);
                    break;
                default:
                    task = await firebaseStorage
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