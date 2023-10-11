using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS
{
    public class APIDefine
    {
        public const string defaultRoute = "api/[controller]";
        public class Member
        {
            public const string Create = defaultRoute;
            public const string Detail = defaultRoute + "/{id}";
            public const string Update =  defaultRoute;
            public const string GetAll = defaultRoute;
        }
    }
}