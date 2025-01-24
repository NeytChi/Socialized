using Microsoft.EntityFrameworkCore;
using Domain.Users;
using Domain.Admins;
using Domain.AutoPosting;
using Domain.GettingSubscribes;
using Domain.Packages;
using Domain.InstagramAccounts;
using Core;

namespace Infrastructure
{
    public partial class Context : DbContext
    {
        private bool useInMemoryDatabase = false;
        private bool useConfiguration = false;

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AppealFile> AppealFiles { get; set; }
        public virtual DbSet<AppealMessage> AppealMessages { get; set; }
        public virtual DbSet<AppealMessageReply> AppealReplies { get; set; }
        public virtual DbSet<Appeal> Appeals { get; set; }
        public virtual DbSet<AutoPostFile> AutoPostFiles { get; set; }
        public virtual DbSet<AutoPost> AutoPosts { get; set; }
        public virtual DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<DiscountPackage> DiscountPackages { get; set; }
        public virtual DbSet<IGAccount> IGAccounts { get; set; }
        public virtual DbSet<PackageAccess> PackageAccess { get; set; }
        public virtual DbSet<ServiceAccess> ServiceAccess { get; set; }
        public virtual DbSet<AccountProfile> SessionProfiles { get; set; }
        public virtual DbSet<SessionState> States { get; set; }
        public virtual DbSet<TaskData> TaskData { get; set; }
        public virtual DbSet<TaskFilter> TaskFilters { get; set; }
        public virtual DbSet<TaskGS> TaskGS { get; set; }
        public virtual DbSet<TaskOption> TaskOptions { get; set; }
        public virtual DbSet<TimeAction> TimeAction { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public Context()
        {

        }
        public Context(bool useInMemoryDatabase)
        {
            this.useInMemoryDatabase = useInMemoryDatabase;
            useConfiguration = true;
        }
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var profileCondition = new ProfileCondition();
            modelBuilder.Entity<PackageAccess>().HasData(
                new PackageAccess 
                { 
                    Id = 1, 
                    Name = "default", 
                    Price = 250, 
                    IGAccounts = 3, 
                    Posts = 10, 
                    Stories = 10, 
                    AnalyticsDays = 30 
                }
            );
            modelBuilder.Entity<DiscountPackage>().HasData(
                new DiscountPackage
                {
                    Id = 1,
                    Percent = 50,
                    Day = 10,
                    Month = 1,
                }
            );
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "user@example.com",
                    FirstName = "default",
                    LastName = "default",
                    Role = "default",
                    Password = profileCondition.HashPassword("Pass1234!"),
                    CreatedAt = DateTime.Now,
                    TokenForStart = ""
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
