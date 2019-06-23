using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SB.Model.Entity;
using System;

namespace SB.Core.Mapping
{
    public abstract class AuditableEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : AuditableEntity
    {
        private readonly bool _isIdentity;
        private readonly string _tableName;
        private readonly string _schemaName;

        public AuditableEntityTypeConfiguration(string tableName, string schemaName = "", bool isIdentity = false)
        {
            this._isIdentity = isIdentity;

            if (tableName.Length > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(tableName), "Tablo adı en fazla 30 karakter içermelidir.");
            }

            this._tableName = tableName;
            this._schemaName = schemaName;
        }

        protected string TableName
        {
            get
            {
                return this._tableName;
            }
        }

        protected string SchemaName
        {
            get
            {
                return this._schemaName;
            }
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            if (string.IsNullOrWhiteSpace(this._schemaName))
            {
                builder.ToTable(this._tableName);
            }
            else
            {
                builder.ToTable(this._tableName, this._schemaName);
            }

            builder.HasKey(x => x.Id);

            if (this._isIdentity)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Hmac).IsRequired();
        }
    }
}
