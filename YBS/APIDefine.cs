using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS
{
    public static class APIDefine
    {
        //  Api Version
        public const string API_VERSION = "api/v{version:apiVersion}";

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
        public const string MEMBER_UPDATE_GUEST = DEFAULT_ROUTE + "/booking" + "/{bookingId}" + "/guests" + "/{guestId}";
        public const string MEMBER_GET_ALL_GUEST_LIST = DEFAULT_ROUTE + "/{memberId}" + "/guests";
        public const string MEMBER_GET_DETAIL_GUEST = DEFAULT_ROUTE + "/booking" + "/{bookingId}" + "/guests" + "/{guestId}";
        public const string MEMBER_GET_ALL_TRIP = DEFAULT_ROUTE + "/trips";

        //Company
        public const string COMPANY_GET_ALL = DEFAULT_ROUTE;
        public const string COMPANY_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string COMPANY_CREATE = DEFAULT_ROUTE;
        public const string COMPANY_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";
        public const string COMPANY_GET_ALL_TRIP = DEFAULT_ROUTE + "/trips";
        public const string COMPANY_GET_ALL_ROUTE = DEFAULT_ROUTE + "/routes";
        public const string COMPANY_GET_ALL_YACHT = DEFAULT_ROUTE + "/yachts";
        public const string COMPANY_GET_ALL_YACHT_TYPE = DEFAULT_ROUTE + "/yacht-types";
        public const string COMPANY_GET_ALL_SERVICE_PACKAGE = DEFAULT_ROUTE + "/service-packages";
        public const string COMPANY_GET_ALL_PRICE_MAPPER = DEFAULT_ROUTE + "/price-mappers" + "/{routeId}";
        public const string COMPANY_GET_ALL_YACHT_MOORING = DEFAULT_ROUTE + "/docks" + "/{dockId}" + "/yacht-moorings";
        public const string COMPANY_UPDATE_REQUEST_CREATE = DEFAULT_ROUTE + "/update-requests";
        public const string COMPANY_UPDATE_REQUEST_GET_DETAIL = DEFAULT_ROUTE + "/update-requests" + "/{id}";
        public const string COMPANY_UPDATE_REQUEST_UPDATE = DEFAULT_ROUTE + "/update-requests" + "/{id}";

        //Route
        public const string ROUTE_GET_ALL = DEFAULT_ROUTE;
        public const string ROUTE_GET_BEGINNING_FILTER = DEFAULT_ROUTE + "/beginning-filter";
        public const string ROUTE_GET_DESTINATION_FILTER = DEFAULT_ROUTE + "/destination-filter";
        public const string ROUTE_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string ROUTE_CREATE = DEFAULT_ROUTE;
        public const string ROUTE_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string ROUTE_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

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
        public const string YACHT_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

        //YachtType
        public const string YACHT_TYPE_DEFAULT = "api/v{version:apiVersion}/yacht-types";
        public const string YACHT_TYPE_GET_ALL = YACHT_TYPE_DEFAULT;
        public const string YACHT_TYPE_GET_DETAIL = YACHT_TYPE_DEFAULT + "/{id}";
        public const string YACHT_TYPE_CREATE = YACHT_TYPE_DEFAULT;
        public const string YACHT_TYPE_UPDATE = YACHT_TYPE_DEFAULT + "/{id}";
        public const string YACHT_TYPE_CHANGE_STATUS = YACHT_TYPE_DEFAULT + "/{id}";

        //Dock
        public const string DOCK_GET_ALL = DEFAULT_ROUTE + "/{companyId}";
        public const string DOCK_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string DOCK_CREATE = DEFAULT_ROUTE;
        public const string DOCK_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string DOCK_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

        //Booking
        public const string BOOKING_GUEST_CREATE = DEFAULT_ROUTE + "/guests";
        public const string BOOKING_MEMBER_CREATE = DEFAULT_ROUTE + "/members";
        public const string BOOKING_GUEST_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";
        public const string BOOKING_GET_ALL = DEFAULT_ROUTE + "/{companyId}";
        public const string BOOKING_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        //Payment
        public const string PAYMENT_BOOKING_CREATE_URL = DEFAULT_ROUTE + "/booking";
        public const string PAYMENT_BOOKING_CALL_BACK = DEFAULT_ROUTE + "/booking";
        public const string PAYMENT_MEMBERSHIP_CREATE_URL = DEFAULT_ROUTE + "/membership";
        public const string PAYMENT_MEMBERSHIP_CALL_BACK = DEFAULT_ROUTE + "/membership";
        //Transaction 
        public const string TRANSACTION_CREATE = DEFAULT_ROUTE;
        public const string TRANSACTION_GET_ALL = DEFAULT_ROUTE + "memberId";

        //MembershipRegostration
        public const string MEMBERSHIP_REGISTRATION = "api/v{version:apiVersion}/membership-registrations";
        public const string MEMBERSHIP_REGISTRATION_GET_ALL = MEMBERSHIP_REGISTRATION;
        public const string MEMBERSHIP_REGISTRATION_DETAIL = MEMBERSHIP_REGISTRATION + "/{id}";

        //bookingPayment
        public const string BOOKING_PAYMENT_GET_ALL = "api/v{version:apiVersion}/company/{companyId}/booking-payments";
        public const string BOOKING_PAYMENT_DETAIL = DEFAULT_ROUTE + "/{id}";
        //wallet
        public const string WALLET_GET_ALL = DEFAULT_ROUTE + "/{memberId}";
        public const string WALLET_GET_DETAIL = DEFAULT_ROUTE + "/{id}";

        //service
        public const string SERVICE_GET_ALL = DEFAULT_ROUTE;
        public const string SERVICE_GET_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string SERVICE_CREATE = DEFAULT_ROUTE;
        public const string SERVICE_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string SERVICE_CHANGE_STATUS = DEFAULT_ROUTE + "/{id}";

        //servicePackage
        public const string SERVICE_PACKAGE_DEFAULT = "api/v{version:apiVersion}/service-packages";
        public const string SERVICE_PACKAGE_GET_ALL = SERVICE_PACKAGE_DEFAULT;
        public const string SERVICE_PACKAGE_GET_DETAIL = SERVICE_PACKAGE_DEFAULT + "/{id}";
        public const string SERVICE_PACKAGE_CREATE = SERVICE_PACKAGE_DEFAULT;
        public const string SERVICE_PACKAGE_UPDATE = SERVICE_PACKAGE_DEFAULT + "/{id}";
        public const string SERVICE_PACKAGE_CHANGE_STATUS = SERVICE_PACKAGE_DEFAULT + "/{id}";
        //Yacht Mooring 
        public const string YACHT_MOORING_CREATE = API_VERSION + "/yacht-moorings";
        public const string YACHT_MOORING_UPDATE = API_VERSION + "/yacht-moorings" + "/{id}";
        //  Deals
        public const string DEALS_DEFAULT = API_VERSION + "/deals";
        // Price Mapper
        public const string PRICE_MAPPER_CREATE = DEFAULT_ROUTE;
        public const string PRICE_MAPPER_UPDATE = DEFAULT_ROUTE + "/{id}";
        public const string PRICE_MAPPER_DETAIL = DEFAULT_ROUTE + "/{id}";
        public const string PRICE_MAPPER_DELETE = DEFAULT_ROUTE + "/{id}";
        //activity
        public const string ACTIVITY_CREATE = DEFAULT_ROUTE;
    }
}