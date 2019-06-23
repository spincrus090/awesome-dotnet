using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SB.Model.Entity;
using System;

namespace SB.Core.Mapping
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        private readonly string _tableName;
        private readonly string _schemaName;
        private readonly bool _isIdentity;

        public BaseEntityTypeConfiguration(string tableName, bool isIdentity, string schemaName = "")
        {
            if (tableName.Length > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(tableName), "Tablo adı en fazla 30 karakter içermelidir.");
            }

            this._tableName = tableName;
            this._schemaName = schemaName;
            this._isIdentity = isIdentity;
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

        protected bool IsIdentity
        {
            get
            {
                return this._isIdentity;
            }
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            if (string.IsNullOrWhiteSpace(this._schemaName))
            {
                builder.ToTable(_tableName);
            }
            else
            {
                builder.ToTable(_tableName, _schemaName);
            }

            if (this._isIdentity)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedNever();
            }
        }
    }
}
