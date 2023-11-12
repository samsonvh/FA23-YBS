using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Utils
{
    public static class FirebaseExtension
    {
        public static string[] GetFullPath(string fileUrl, string prefixUrl)
        {
            //remove prefixUrl 
            var oldPath = fileUrl.Remove(0, prefixUrl.Length);
            //find postfix index
            var postFixIndex = oldPath.IndexOf("?");
            //remove postfix
            oldPath = oldPath.Remove(postFixIndex, oldPath.Length - postFixIndex);
            //Replace %2F to /
            oldPath = oldPath.Replace("%2F", "/");
            //Split path 
            var resultSplit = oldPath.Split('/');
            return resultSplit;
        }
    }
}