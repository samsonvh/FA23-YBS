using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Context;
using YBS.Data.Models;
using YBS.Data.Repositories.Implements;
using YBS.Data.Repositories.Interfaces;
using YBS.Data.UniOfWork.Interfaces;

namespace YBS.Data.UniOfWork.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YBSContext _context;
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IGenericRepository<Dock> _dockRepository;
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Route> _routeRepository;

        public UnitOfWork(YBSContext context)
        {
            _context = context;
        }

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                if(_accountRepository is not null)
                {
                    return _accountRepository;
                }
                return new GenericRepository<Account>(_context);
            }
        }

        public IGenericRepository<Company> CompanyRepository
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

        public IGenericRepository<Dock> DockRepository
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

        public IGenericRepository<Member> MemberRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _memberRepository;
                }
                return new GenericRepository<Member>(_context);
            }
        }

        public IGenericRepository<Role> RoleRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _roleRepository;
                }
                return new GenericRepository<Role>(_context);
            }
        }

        public IGenericRepository<Route> RouteRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _routeRepository;
                }
                return new GenericRepository<Route>(_context);
            }
        }

        public Task<int> Commit()
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
