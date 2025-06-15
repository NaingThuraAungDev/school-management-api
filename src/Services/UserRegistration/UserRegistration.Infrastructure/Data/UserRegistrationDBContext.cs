using Microsoft.EntityFrameworkCore;
using UserRegistration.Domain.Entities;

namespace UserRegistration.Infrastructure.Data
{
    public class UserRegistrationDBContext : DbContext
    {
        public UserRegistrationDBContext(DbContextOptions<UserRegistrationDBContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRegistrationDBContext).Assembly);
        }
    }
}