using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Context;
using YBS.Data.Models;
using YBS.Data.Repositories;
using YBS.Data.Repositories.Implements;

namespace YBS.Data.UnitOfWorks.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YBSContext _context;
        private readonly IGenericRepositoty<Account> _accountRepository;
        private readonly IGenericRepositoty<Member> _memberRepository;
        private readonly IGenericRepositoty<Company> _companyRepository;
        private readonly IGenericRepositoty<MembershipPackage> _membershipPackageRepository;
        private readonly IGenericRepositoty<Role> _roleRepository;
        private readonly IGenericRepositoty<RefreshToken> _refreshTokenRepository;
        private readonly IGenericRepositoty<Yacht> _yachtRepository;
        private readonly IGenericRepositoty<YachtType> _yachtTypeRepository;
        private readonly IGenericRepositoty<Route> _routeRepository;
        private readonly IGenericRepositoty<Booking> _bookingRepository;
        private readonly IGenericRepositoty<Trip> _tripRepository;
        private readonly IGenericRepositoty<PriceMapper> _priceMapperRepository;
        private readonly IGenericRepositoty<Guest> _guestRepository;
        private readonly IGenericRepositoty<ServicePackage> _servicePackageRepository;
        private readonly IGenericRepositoty<Dock> _dockRepository;
        private readonly IGenericRepositoty<BookingPayment> _bookingPaymentRepository;
        private readonly IGenericRepositoty<Transaction> _transactionRepository;
        private readonly IGenericRepositoty<MembershipRegistration> _membershipRegistrationRepository;
        private readonly IGenericRepositoty<Wallet> _walletRepository;
        private readonly IGenericRepositoty<Service> _serviceRepository;
        private readonly IGenericRepositoty<ServicePackageItem> _servicePackageItemRepository;
        private readonly IGenericRepositoty<UpdateRequest> _updateRequestRepository;
        private readonly IGenericRepositoty<DockYachtType> _dockYachtTypeRepository;
        private readonly IGenericRepositoty<YachtMooring> _yachtMooringRepository;
        private readonly IGenericRepositoty<Activity> _activityRepository;

        public UnitOfWork(YBSContext context)
        {
            _context = context;
        }
        public IGenericRepositoty<Account> AccountRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _accountRepository;
                }
                return new GenericRepository<Account>(_context);
            }
        }
        public IGenericRepositoty<Member> MemberRepository
        {
            get
            {
                if (_memberRepository is not null)
                {
                    return _memberRepository;
                }
                return new GenericRepository<Member>(_context);
            }
        }

        public IGenericRepositoty<Company> CompanyRepository
        {
            get
            {
                if (_companyRepository is not null)
                {
                    return _companyRepository;
                }
                return new GenericRepository<Company>(_context);
            }
        }

        public IGenericRepositoty<MembershipPackage> MembershipPackageRepository
        {
            get
            {
                if (_membershipPackageRepository is not null)
                {
                    return _membershipPackageRepository;
                }
                return new GenericRepository<MembershipPackage>(_context);
            }
        }
        public IGenericRepositoty<Role> RoleRepository
        {
            get
            {
                if (_roleRepository is not null)
                {
                    return _roleRepository;
                }
                return new GenericRepository<Role>(_context);
            }
        }

        public IGenericRepositoty<RefreshToken> RefreshTokenRepository 
        {
            get
            {
                if (_refreshTokenRepository is not null)
                {
                    return _refreshTokenRepository;
                }
                return new GenericRepository<RefreshToken>(_context);
            }
        }

        public IGenericRepositoty<Yacht> YachRepository
        {
            get
            {
                if (_yachtRepository is not null)
                {
                    return _yachtRepository;
                }
                return new GenericRepository<Yacht>(_context);
            }
        }

        public IGenericRepositoty<YachtType> YachTypeRepository
        {
            get
            {
                if (_yachtTypeRepository is not null)
                {
                    return _yachtTypeRepository;
                }
                return new GenericRepository<YachtType>(_context);
            }
        }

        public IGenericRepositoty<Route> RouteRepository
        {
            get
            {
                if (_routeRepository is not null)
                {
                    return _routeRepository;
                }
                return new GenericRepository<Route>(_context);
            }
        }

        public IGenericRepositoty<Booking> BookingRepository
        {
            get
            {
                if (_bookingRepository is not null)
                {
                    return _bookingRepository;
                }
                return new GenericRepository<Booking>(_context);
            }
        }

        public IGenericRepositoty<Trip> TripRepository
        {
            get
            {
                if (_tripRepository is not null)
                {
                    return _tripRepository;
                }
                return new GenericRepository<Trip>(_context);
            }
        }

        public IGenericRepositoty<PriceMapper> PriceMapperRepository
        {
            get
            {
                if (_priceMapperRepository is not null)
                {
                    return _priceMapperRepository;
                }
                return new GenericRepository<PriceMapper>(_context);
            }
        }

        public IGenericRepositoty<Guest> GuestRepository
        {
            get
            {
                if (_guestRepository is not null)
                {
                    return _guestRepository;
                }
                return new GenericRepository<Guest>(_context);
            }
        }

        public IGenericRepositoty<ServicePackage> ServicePackageRepository
        {
            get
            {
                if (_servicePackageRepository is not null)
                {
                    return _servicePackageRepository;
                }
                return new GenericRepository<ServicePackage>(_context);
            }
        }

        public IGenericRepositoty<Dock> DockRepository
        {
            get
            {
                if (_dockRepository is not null)
                {
                    return _dockRepository;
                }
                return new GenericRepository<Dock>(_context);
            }
        }
        public IGenericRepositoty<BookingPayment> BookingPaymentRepository
        {
            get
            {
                if (_bookingPaymentRepository is not null)
                {
                    return _bookingPaymentRepository;
                }
                return new GenericRepository<BookingPayment>(_context);
            }
        }

        public IGenericRepositoty<Transaction> TransactionRepository
        {
            get
            {
                if (_transactionRepository is not null)
                {
                    return _transactionRepository;
                }
                return new GenericRepository<Transaction>(_context);
            }
        }

        public IGenericRepositoty<MembershipRegistration> MembershipRegistrationRepository
        {
            get
            {
                if (_membershipRegistrationRepository is not null)
                {
                    return _membershipRegistrationRepository;
                }
                return new GenericRepository<MembershipRegistration>(_context);
            }
        }

        public IGenericRepositoty<Wallet> WalletRepository
        {
            get
            {
                if (_walletRepository is not null)
                {
                    return _walletRepository;
                }
                return new GenericRepository<Wallet>(_context);
            }
        }

        public IGenericRepositoty<Service> ServiceRepository
        {
            get
            {
                if (_serviceRepository is not null)
                {
                    return _serviceRepository;
                }
                return new GenericRepository<Service>(_context);
            }
        }

        public IGenericRepositoty<ServicePackageItem> ServicePackageItemRepository
        {
            get
            {
                if (_servicePackageItemRepository is not null)
                {
                    return _servicePackageItemRepository;
                }
                return new GenericRepository<ServicePackageItem>(_context);
            }
        }

        public IGenericRepositoty<UpdateRequest> UpdateRequestRepository
        {
            get
            {
                if (_updateRequestRepository is not null)
                {
                    return _updateRequestRepository;
                }
                return new GenericRepository<UpdateRequest>(_context);
            }
        }

        public IGenericRepositoty<DockYachtType> DockYachtTypeRepository
        {
            get
            {
                if (_dockYachtTypeRepository is not null)
                {
                    return _dockYachtTypeRepository;
                }
                return new GenericRepository<DockYachtType>(_context);
            }
        }

        public IGenericRepositoty<YachtMooring> YachtMooringRepository
        {
            get
            {
                if (_yachtMooringRepository is not null)
                {
                    return _yachtMooringRepository;
                }
                return new GenericRepository<YachtMooring>(_context);
            }
        }

        public IGenericRepositoty<Activity> ActivityRepository
        {
            get
            {
                if (_activityRepository is not null)
                {
                    return _activityRepository;
                }
                return new GenericRepository<Activity>(_context);
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}