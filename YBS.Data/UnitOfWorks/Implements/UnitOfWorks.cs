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
    public class UnitOfWorks
    {
        private readonly YBSContext _context;
        private readonly IGenericRepositories<Account> _accountRepository;
        private readonly IGenericRepositories<Company> _companyRepository;
        private readonly IGenericRepositories<Member> _memberRepository;
        private readonly IGenericRepositories<MembershipPackage> _membershipPackageRepository;
        private readonly IGenericRepositories<Role> _roleRepository;
        public UnitOfWorks(YBSContext context)
        {
            _context = context;
        }
        public IGenericRepositories<Account> AccountRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _accountRepository;
                }
                return new GenericRepositories<Account>(_context);
            }
        }
        public IGenericRepositories<Company> CompanyRepository
        {
            get
            {
                if (_companyRepository is not null)
                {
                    return _companyRepository;
                }
                return new GenericRepositories<Company>(_context);
            }
        }
        public IGenericRepositories<Member> MemberRepository
        {
            get
            {
                if (_memberRepository is not null)
                {
                    return _memberRepository;
                }
                return new GenericRepositories<Member>(_context);
            }
        }
        public IGenericRepositories<MembershipPackage> MembershipPackageRepository
        {
            get
            {
                if (_membershipPackageRepository is not null)
                {
                    return _membershipPackageRepository;
                }
                return new GenericRepositories<MembershipPackage>(_context);
            }
        }
        public IGenericRepositories<Role> RoleRepository
        {
            get
            {
                if (_roleRepository is not null)
                {
                    return _roleRepository;
                }
                return new GenericRepositories<Role>(_context);
            }
        }
    }
}