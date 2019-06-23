using Microsoft.EntityFrameworkCore.Storage;
using SB.Core.Context;
using System;
using System.Threading.Tasks;

namespace SB.Core.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IDbContext DbContext { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void CommitTransactionAsync();
        void Rollback();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        int SaveChanges(Guid userId);
        Task<int> SaveChangesAsync(Guid userId);
    }
}
