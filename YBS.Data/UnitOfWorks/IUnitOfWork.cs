using YBS.Data.Models;
using YBS.Data.Repositories;

namespace YBS.Data.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IGenericRepositoty<Account> AccountRepository { get; }
        IGenericRepositoty<Member> MemberRepository { get; }
        IGenericRepositoty<Company> CompanyRepository { get; }
        IGenericRepositoty<MembershipPackage> MembershipPackageRepository { get; }
        IGenericRepositoty<Role> RoleRepository { get; }
        IGenericRepositoty<RefreshToken> RefreshTokenRepository { get; }
        IGenericRepositoty<Yacht> YachRepository { get; }
        IGenericRepositoty<YachtType> YachTypeRepository { get; }
        IGenericRepositoty<Route> RouteRepository { get; }
        Task<int> SaveChangesAsync();
    }
}