using Domain.Entities;
using Domain.Multitenancy;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ULMS.Infrastructure.Persistence.Context.Extensions;

namespace Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;
        private const string IsDeletedProperty = "IsDeleted";

        public ApplicationContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TID;
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurations<ApplicationContext>();
            modelBuilder.ConfigureDeletableEntities();
            modelBuilder.Entity<Topic>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Category>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Course>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Module>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Learner>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Instructor>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<InstructorCourse>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<LearnerCourse>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<User>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Role>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<UserRole>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Admin>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<CourseConstant>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<CourseRequest>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<Assignment>().HasQueryFilter(a => a.TenantId == TenantId);
            modelBuilder.Entity<LearnerAssignment>().HasQueryFilter(a => a.TenantId == TenantId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        entry.CurrentValues[IsDeletedProperty] = false;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.TenantId = TenantId;
                        entry.CurrentValues[IsDeletedProperty] = true;
                        break;

                }
            }
            this.AddAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<InstructorCourse> InstructorCourses { get; set; }
        public DbSet<LearnerCourse> LearnerCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseRequest> CourseRequests { get; set; }
        public DbSet<CourseConstant> CourseConstants { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<LearnerAssignment> LearnerAssignments { get; set; }
    }
}
