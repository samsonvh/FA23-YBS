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
        private readonly IGenericRepositoty<Company> _companyRepository;
        private readonly IGenericRepositoty<UpdateRequest> _updateRequestRepository;
        private readonly IGenericRepositoty<Member> _memberRepository;
        private readonly IGenericRepositoty<MembershipPackage> _membershipPackageRepository;
        private readonly IGenericRepositoty<Role> _roleRepository;
        private readonly IGenericRepositoty<RefreshToken> _refreshTokenRepository;
        private readonly IGenericRepositoty<Dock> _dockRepository;
        private readonly IGenericRepositoty<Yacht> _yachtRepository;
        private readonly IGenericRepositoty<YachtType> _yachtTypeRepository;

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

        public IGenericRepositoty<Dock> DockRepository
        {
            get
            {
                if(_dockRepository is not null)
                {
                    return _dockRepository;
                }
                return new GenericRepository<Dock>(_context);
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