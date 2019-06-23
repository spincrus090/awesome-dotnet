using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SB.Core.Mapping;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationDomain.Mapping
{
    public class UserMapping : AuditableEntityTypeConfiguration<User>
    {
        public UserMapping() : base("User", "Authentication")
        {
        }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Username).HasMaxLength(50);
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
        }
    }
}
