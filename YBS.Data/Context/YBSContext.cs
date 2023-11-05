using Microsoft.EntityFrameworkCore;
using YBS.Data.Models;

namespace YBS.Data.Context
{
    public class YBSContext : DbContext
    {
        public YBSContext(DbContextOptions<YBSContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UpdateRequest> UpdateRequests { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipPackage> MembershipPackages { get; set; }
        public DbSet<MembershipRegistration> MembershipRegistrations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Dock> Docks { get; set; }
        public DbSet<Yacht> Yachts { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<DockYachtType> DockYachtTypes { get; set; }
        public DbSet<YachtType> YachtTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteServicePackage> RouteServicePackages { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
        public DbSet<BookingServicePackage> BookingServicePackages { get; set; }
        public DbSet<ServicePackageItem> ServicePackageItems { get; set; }
        public DbSet<PriceMapper> PriceMappers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<BookingPayment> BookingPayments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityPlace> ActivityPlaces { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YBSContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
