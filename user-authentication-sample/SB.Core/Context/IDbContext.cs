using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SB.Model.Entity;
using System;
using System.Threading.Tasks;

namespace SB.Core.Context
{
    public interface IDbContext
    {
        DbSet<ChangeLog> ChangeLogs { get; set; }
        DatabaseFacade Database { get; }
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        int SaveChanges(Guid userId);
        Task<int> SaveChangesAsync(Guid userId);
        void Dispose();
    }
}
