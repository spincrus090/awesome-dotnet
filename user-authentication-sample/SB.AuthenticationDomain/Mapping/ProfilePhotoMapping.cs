using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SB.Core.Mapping;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationDomain.Mapping
{
    public class ProfilePhotoMapping : AuditableEntityTypeConfiguration<ProfilePhoto>
    {
        public ProfilePhotoMapping() : base("ProfilPhoto", "Authentication")
        {
        }

        public override void Configure(EntityTypeBuilder<ProfilePhoto> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Photo);

            builder.HasOne(x => x.User)
                .WithOne(x => x.ProfilePhoto)
                .HasForeignKey<ProfilePhoto>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
