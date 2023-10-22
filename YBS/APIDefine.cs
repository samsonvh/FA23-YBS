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
        //Account
        public const string ACCOUNT_GET_ALL = DEFAULT_ROUTE;
        //Authentication
        public const string GOOGLE_LOGIN = "api/[controller]" + "/google-login";
        public const string REFRESH_TOKEN = "api/[controller]" + "/refresh-token";
        public const string GOOGLE_AUTHENTICATION = "api/[controller]" + "/";
        //Member
        public const string MEMBER_CREATE = DEFAULT_ROUTE;
        public const string MEMBER_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string MEMBER_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string MEMBER_GET_ALL = DEFAULT_ROUTE;
        //Company
        public const string COMPANY_GET_ALL = DEFAULT_ROUTE;
        public const string COMPANY_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string COMPANY_CREATE = DEFAULT_ROUTE;
        public const string COMPANY_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

        //Membership Package
        public const string MEMBERSHIP_PACKAGE = "membership-packages";
        public const string MEMBERSHIP_PACKAGE_CREATE = MEMBERSHIP_PACKAGE;
        public const string MEMBERSHIP_PACKAGE_DETAIL = MEMBERSHIP_PACKAGE + "/{id}";
        public const string MEMBERSHIP_PACKAGE_UPDATE = MEMBERSHIP_PACKAGE + "/{id}";
        public const string MEMBERSHIP_PACKAGE_GET_ALL = MEMBERSHIP_PACKAGE;
        //Yacht
        public const string YACHT_CREATE = DEFAULT_ROUTE;
        public const string YACHT_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string YACHT_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string YACHT_GET_ALL = DEFAULT_ROUTE;
        //Booking
        public const string BOOKING_GUEST_CREATE = DEFAULT_ROUTE + "/guests";
        public const string BOOKING_GUEST_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";
        public const string BOOKING_GET_ALL = DEFAULT_ROUTE;
        public const string BOOKING_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
    }
}