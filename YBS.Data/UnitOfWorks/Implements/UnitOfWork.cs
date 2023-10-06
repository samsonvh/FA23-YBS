using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Context;
using YBS.Data.Models;
using YBS.Data.Repositories.Implements;
using YBS.Data.Repositories;

namespace YBS.Data.UnitOfWorks.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YBSContext _context;

        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IGenericRepository<Member> _memberRepository;

        public UnitOfWork(YBSContext context)
        {
            _context = context;
        }

        public IGenericRepository<Role> RoleRepository
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

        public IGenericRepository<Account> AccountRepository
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

        public IGenericRepository<Member> MemberRepository
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
