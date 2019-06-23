using Microsoft.EntityFrameworkCore.Storage;
using SB.Core.Context;
using SB.Core.UnitOfWork.Interface;
using System;
using System.Threading.Tasks;

namespace SB.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;
        private IDbContextTransaction _dbContextTransaction;

        public UnitOfWork(IDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IDbContext DbContext
        {
            get
            {
                return this._dbContext;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            this._dbContextTransaction = await this._dbContext.Database.BeginTransactionAsync();
            return this._dbContextTransaction;
        }

        public IDbContextTransaction BeginTransaction()
        {
            this._dbContextTransaction = this._dbContext.Database.BeginTransaction();
            return this._dbContextTransaction;
        }

        public void CommitTransaction()
        {
            try
            {
                this._dbContext.Database.CommitTransaction();
                this._dbContext.SaveChanges();
                this._dbContextTransaction.Dispose();
            }
            catch (Exception ex)
            {
                this.Rollback();
                throw ex;
            }
        }

        public async void CommitTransactionAsync()
        {
            try
            {
                this._dbContext.Database.CommitTransaction();
                await this._dbContext.SaveChangesAsync();
                this._dbContextTransaction.Dispose();
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            this._dbContext.Database.RollbackTransaction();
            this._dbContextTransaction.Dispose();
        }

        public int SaveChanges()
        {
            return this._dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return this._dbContext.SaveChangesAsync();
        }

        public int SaveChanges(Guid userId)
        {
            return this._dbContext.SaveChanges(userId);
        }

        public async Task<int> SaveChangesAsync(Guid userId)
        {
            return await this._dbContext.SaveChangesAsync(userId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(obj: this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dbContext != null)
                {
                    this._dbContext.Dispose();
                    this._dbContext = null;
                }
            }
        }
    }
}
