using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS
{
    public static class APIDefine
    {
        //Default Route
        public const string DEFAULT_ROUTE = "api/v{version:apiVersion}/[controller]";
        //Account
        public const string ACCOUNT_GET_ALL = DEFAULT_ROUTE;
        public const string ACCOUNT_DETAIL = DEFAULT_ROUTE + "/{id}";
        //Authentication
        public const string GOOGLE_LOGIN = DEFAULT_ROUTE + "/google-login";
        public const string LOGIN = DEFAULT_ROUTE + "/login";
        public const string REFRESH_TOKEN = DEFAULT_ROUTE + "/refresh-token";
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

        //Route
        public const string ROUTE_GET_ALL = DEFAULT_ROUTE;
        public const string ROUTE_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string ROUTE_CREATE= DEFAULT_ROUTE;
        public const string ROUTE_UPDATE = DEFAULT_ROUTE + "/{id}";

        //Membership Package
        public const string MEMBERSHIP_PACKAGE = "api/v{version:apiVersion}/membership-packages";
        public const string MEMBERSHIP_PACKAGE_CREATE = MEMBERSHIP_PACKAGE;
        public const string MEMBERSHIP_PACKAGE_DETAIL = MEMBERSHIP_PACKAGE + "/{id}";
        public const string MEMBERSHIP_PACKAGE_UPDATE = MEMBERSHIP_PACKAGE + "/{id}";
        public const string MEMBERSHIP_PACKAGE_GET_ALL = MEMBERSHIP_PACKAGE;
        public const string MEMBERSHIP_PACKAGE_CHANGE_STATUS = MEMBERSHIP_PACKAGE + "/{id}";

        //Yacht
        public const string YACHT_CREATE = DEFAULT_ROUTE;
        public const string YACHT_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string YACHT_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string YACHT_GET_ALL = DEFAULT_ROUTE;

        //YachtType
        public const string YACHT_TYPE_DEFAULT = "api/v{version:apiVersion}/yacht-types";
        public const string YACHT_TYPE_GET_ALL = YACHT_TYPE_DEFAULT;
        public const string YACHT_TYPE_GET_DETAIL = YACHT_TYPE_DEFAULT + "/{id}";
        public const string YACHT_TYPE_CREATE = YACHT_TYPE_DEFAULT;
        public const string YACHT_TYPE_UPDATE = YACHT_TYPE_DEFAULT + "/{id}";
        public const string YACHT_TYPE_CHANGE_STATUS = YACHT_TYPE_DEFAULT + "/{id}";

        //Dock
        public const string DOCK_GET_ALL = DEFAULT_ROUTE;
        public const string DOCK_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string DOCK_CREATE = DEFAULT_ROUTE;
        public const string DOCK_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

        //Booking
        public const string BOOKING_GUEST_CREATE = DEFAULT_ROUTE + "/guests";
        public const string BOOKING_MEMBER_CREATE = DEFAULT_ROUTE + "/members";
        public const string BOOKING_GUEST_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";
        public const string BOOKING_GET_ALL = DEFAULT_ROUTE;
        public const string BOOKING_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        //Payment
        public const string PAYMENT_BOOKING_CREATE_URL = DEFAULT_ROUTE + "/booking";
        public const string PAYMENT_BOOKING_CALL_BACK = DEFAULT_ROUTE + "/booking";
        public const string PAYMENT_MEMBERSHIP_CREATE_URL = DEFAULT_ROUTE + "/membership";
        public const string PAYMENT_MEMBERSHIP_CALL_BACK = DEFAULT_ROUTE + "/membership";
        //Transaction 
        public const string TRANSACTION_CREATE = DEFAULT_ROUTE;
        public const string TRANSACTION_GET_ALL = DEFAULT_ROUTE;

        //MembershipRegostration
        public const string MEMBERSHIP_REGISTRATION = "api/v{version:apiVersion}/membership-registrations";
        public const string MEMBERSHIP_REGISTRATION_GET_ALL = MEMBERSHIP_REGISTRATION;
        public const string MEMBERSHIP_REGISTRATION_DETAIL = MEMBERSHIP_REGISTRATION + "/{id}";

        //bookingPayment
        public const string BOOKING_PAYMENT_GET_ALL = "api/v{version:apiVersion}/booking-payments";

        //wallet
        public const string WALLET_GET_ALL = DEFAULT_ROUTE;
        public const string WALLET_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
    }
}