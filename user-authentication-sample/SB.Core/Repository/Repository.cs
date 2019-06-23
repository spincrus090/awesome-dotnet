using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SB.Core.Context;
using SB.Core.Repository.Interface;
using SB.Core.UnitOfWork.Interface;
using SB.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SB.Core.Repository
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private IDbContext _dbContext;

        public Repository(IUnitOfWork unitOfWork)
        {
            this._dbContext = unitOfWork.DbContext;
        }

        protected IDbContext Context
        {
            get { return this._dbContext; }
        }

        protected DbSet<T> Entities
        {
            get { return this.Context.Set<T>(); }
        }

        public IEnumerable<T> GetAll()
        {
            return this.Entities.AsEnumerable();
        }

        public T Get(Guid id)
        {
            return this.Entities.FirstOrDefault(e => e.Id.Equals(id));
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Where(predicate).AsEnumerable();
        }

        public virtual T Insert(T entity)
        {
            EntityEntry<T> entityEntry = this.Entities.Add(entity);
            return entityEntry.Entity;
        }

        public virtual T Update(T entity)
        {
            this.Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual T Delete(T entity)
        {
            this.Context.Entry(entity).State = EntityState.Deleted;
            return entity;
        }
    }
}
