using YBS.Data.Models;
using YBS.Data.Repositories;

namespace YBS.Data.UnitOfWorks
{
    public interface IUnitOfWorks
    {
        IGenericRepositories<Account> AccountRepository { get; }
        IGenericRepositories<Company> CompanyRepository { get; }
        IGenericRepositories<UpdateRequest> UpdateRequestRepository { get; }
        IGenericRepositories<Member> MemberRepository { get; }
        IGenericRepositories<MembershipPackage> MembershipPackageRepository { get; }
        IGenericRepositories<Role> RoleRepository { get; }
        IGenericRepositories<RefreshToken> RefreshTokenRepository { get; }
        IGenericRepositories<Dock> DockRepository { get; }
        IGenericRepositories<Yacht> YachRepository { get; }
        Task<int> SaveChangesAsync();
    }
}