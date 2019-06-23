using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SB.Enums;
using SB.Model.Entity;
using SB.Model.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SB.Core.Context
{
    public class BaseContext : DbContext, IDbContext
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly string _connectionString;

        protected BaseContext(DatabaseProvider databaseProvider, string connectionString)
        {
            _databaseProvider = databaseProvider;
            _connectionString = connectionString;
        }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public int SaveChanges(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return base.SaveChanges();
            }

            this.TrackAuditableChanges(userId);

            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return await base.SaveChangesAsync();
            }

            this.TrackAuditableChanges(userId);
            return await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                switch (_databaseProvider)
                {
                    case DatabaseProvider.MicrosoftSQLServer:
                        optionsBuilder.UseSqlServer(_connectionString);
                        break;
                    case DatabaseProvider.MySQL:
                        break;
                    case DatabaseProvider.Sqlite:
                        break;
                    case DatabaseProvider.InMemory:
                        optionsBuilder.UseInMemoryDatabase("InMemoryDB");
                        break;
                    case DatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(_connectionString);
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Bağlamda yapılan denetime tabi tüm değişikliklerin CHANGELOG tablosuna işler.
        /// </summary>
        /// <param name="userId">İşlemi gerçekleştiren kullanıcının birincil anahtar değeri.</param>
        private void TrackAuditableChanges(Guid userId)
        {
            // Değişikliğe uğramış veriler.
            List<EntityEntry<IAuditableEntity>> entries = this.ChangeTracker.Entries<IAuditableEntity>()
                                                                            .Where(entityEntry => entityEntry.State == EntityState.Modified ||
                                                                                                  entityEntry.State == EntityState.Deleted ||
                                                                                                  entityEntry.State == EntityState.Added).ToList();
            DateTime now = DateTime.UtcNow;

            foreach (EntityEntry<IAuditableEntity> entry in entries)
            {
                // Değişikliği yapan kullanıcı ve ilgili tarih alanları işlenir.
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                }

                Type type = entry.Entity.GetType();

                string entityName = type.Name;
                string primaryKey = this.GetPrimaryKeyValue(entry);

                if (entry.State == EntityState.Added)
                {
                    // Yeni eklenmiş denetime tabi verinin her bir özelliği ChangeLog tablosuna eklenir.
                    foreach (string propertyName in entry.CurrentValues.Properties.Select(x => x.Name))
                    {
                        // yeni değer
                        string currentValue = entry.CurrentValues[propertyName].ToString();
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            Operation = EntityOperation.Added,
                            PrimaryKey = primaryKey,
                            PropertyName = propertyName,
                            NewValue = currentValue,
                            OldValue = string.Empty,
                            ChangedBy = userId.ToString(),
                            ChangedDate = now
                        };
                        this.ChangeLogs.Add(log);
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Silinmiş denetime tabi verinin her bir özelliği ChangeLog tablosuna eklenir.
                    foreach (string propertyName in entry.OriginalValues.Properties.Select(x => x.Name))
                    {
                        // eski değer
                        string currentValue = entry.CurrentValues[propertyName].ToString();
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            Operation = EntityOperation.Deleted,
                            PrimaryKey = primaryKey,
                            PropertyName = propertyName,
                            NewValue = string.Empty,
                            OldValue = currentValue,
                            ChangedBy = userId.ToString(),
                            ChangedDate = now
                        };
                        this.ChangeLogs.Add(log);
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Güncellenmiş denetime tabi verinin her bir özelliğin eski ve yeni değerleri ChangeLog tablosuna eklenir.
                    foreach (string propertyName in entry.OriginalValues.Properties.Select(x => x.Name))
                    {
                        // eski değer
                        string originalValue = entry.OriginalValues[propertyName].ToString();

                        // yeni değer
                        string currentValue = entry.CurrentValues[propertyName].ToString();

                        if (originalValue != currentValue)
                        {
                            ChangeLog log = new ChangeLog()
                            {
                                EntityName = entityName,
                                PrimaryKey = primaryKey,
                                Operation = EntityOperation.Modified,
                                PropertyName = propertyName,
                                OldValue = originalValue,
                                NewValue = currentValue,
                                ChangedBy = userId.ToString(),
                                ChangedDate = now
                            };
                            this.ChangeLogs.Add(log);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parametre olarak verilen varlık nesnesine ait erişim nesnesinin birincil anahtar değerini verir.
        /// </summary>
        /// <param name="entry">Varlık nesnesi ile ilgili bilgilere sahip olan erişim nesnesi.</param>
        /// <returns>Varlık nesnesinin birincil anahtar değeri.</returns>
        private string GetPrimaryKeyValue(EntityEntry entry)
        {
            Type entityType = entry.Entity.GetType();
            List<PropertyInfo> primaryKeyProperties = this.Model.FindEntityType(entityType).FindPrimaryKey().Properties.Select(x => x.PropertyInfo).ToList();
            return string.Join(",", primaryKeyProperties.Select(x => x.GetValue(entry.Entity).ToString()));
        }
    }
}
