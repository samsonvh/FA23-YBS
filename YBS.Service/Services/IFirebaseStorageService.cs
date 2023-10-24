using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YBS.Service.Services
{
    public interface IFirebaseStorageService
    {
        public Task<Uri> UploadFile(string name, IFormFile image, int counter, string objectType, string type = null);
    }
}