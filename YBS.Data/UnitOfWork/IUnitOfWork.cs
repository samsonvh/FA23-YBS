using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.Repositories;

namespace YBS.Data.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Member> MemberRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        Task<int> Commit();
    }
}
