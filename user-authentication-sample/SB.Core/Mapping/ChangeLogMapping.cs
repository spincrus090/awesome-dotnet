using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SB.Model.Entity;

namespace SB.Core.Mapping
{
    public class ChangeLogMapping : BaseEntityTypeConfiguration<ChangeLog>
    {
        #region -- Constructors --

        /// <summary>
        /// Grup verilerinin yapılandırma ayarlarını gerçekleştirir.
        /// </summary>
        public ChangeLogMapping()
            : base("ChangeLog", true, "Logging")
        {
        }

        #endregion

        public override void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            builder.Property(x => x.EntityName).IsRequired().HasMaxLength(511);
            builder.Property(x => x.Operation).IsRequired();
            builder.Property(x => x.PrimaryKey).IsRequired().HasMaxLength(511);
            builder.Property(x => x.PropertyName).IsRequired().HasMaxLength(511);
            builder.Property(x => x.OldValue).IsRequired();
            builder.Property(x => x.NewValue).IsRequired();
            builder.Property(x => x.ChangedBy).IsRequired().HasMaxLength(511);
            builder.Property(x => x.ChangedDate).IsRequired();
        }
    }
}
