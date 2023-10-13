using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS
{
    public static class APIDefine
    {
        //Default Route
        public const string DEFAULT_ROUTE = "api/[controller]";
        //Authentication
        public const string GOOGLE_LOGIN = "api/[controller]" + "/google-login";
        public const string REFRESH_TOKEN = "api/[controller]" + "/refresh-token";
        public const string GOOGLE_AUTHENTICATION = "api/[controller]" + "/";
        //Member
        public const string MEMBER_CREATE = DEFAULT_ROUTE;
        public const string MEMBER_DETAIL = DEFAULT_ROUTE + "/{Id}";
        public const string MEMBER_UPDATE = DEFAULT_ROUTE;
        public const string MEMBER_GET_ALL = DEFAULT_ROUTE;
        //Membership Package
        public const string MEMBERSHIP_PACKAGE_CREATE = DEFAULT_ROUTE;
        public const string MEMBERSHIP_PACKAGE_DETAIL = DEFAULT_ROUTE + "/{Id}";
        public const string MEMBERSHIP_PACKAGE_UPDATE = DEFAULT_ROUTE;
        public const string MEMBERSHIP_PACKAGE_GET_ALL = DEFAULT_ROUTE;
    }
}