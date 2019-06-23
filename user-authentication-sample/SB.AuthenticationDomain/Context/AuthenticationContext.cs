using Microsoft.EntityFrameworkCore;
using SB.AuthenticationDomain.Mapping;
using SB.Core.Context;
using SB.Enums;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationDomain.Context
{
    public class AuthenticationContext : BaseContext, IDbContext
    {
        public AuthenticationContext(DatabaseProvider databaseProvider, string connectionString)
            : base(databaseProvider, connectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProfilePhoto> ProfilPhotos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ProfilePhotoMapping());
        }
    }
}
