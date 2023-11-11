using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YBS.Service.Services
{
    public interface IFirebaseStorageService
    {
        public Task<Uri> UploadFile(string name, IFormFile image, string objectType, string type = null);
        public Task<Uri> RenamedFolder(string oldName,string oldFileName , string oldObjectType, string name, IFormFile image, string objectType, string oldType = null,  string type = null);
    }
}