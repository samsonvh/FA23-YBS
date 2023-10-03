using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.DesignPattern.Repositories.Interfaces;
using YBS.Data.Models;


namespace YBS.Data.DesignPattern.UniOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Dock> DockRepository { get; }
        IGenericRepository<Member> MemberRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Route> RouteRepository { get; }
        Task<int> Commit();

    }
}
