using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SB.Core.Context;
using SB.Core.Repository.Interface;
using SB.Core.UnitOfWork.Interface;
using SB.Enums;
using SB.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SB.Core.Repository
{
    public class AuditableRepository<T> : IAuditableRepository<T>
        where T : AuditableEntity
    {
        private IDbContext _dbContext;

        public AuditableRepository(IUnitOfWork unitOfWork)
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

        public T Delete(T entity, bool isHardDelete = false)
        {
            entity.Status = isHardDelete ? EntityStatus.HardDeleted : EntityStatus.SoftDeleted;
            return this.Update(entity);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available)
        {
            return this.Entities.Where(e => e.Status == entityStatus).Where(predicate).AsEnumerable();
        }

        public T Get(Guid id, EntityStatus entityStatus = EntityStatus.Available)
        {
            return this.Entities.FirstOrDefault(e => e.Status == entityStatus && e.Id.Equals(id));
        }

        public IEnumerable<T> GetAll(EntityStatus entityStatus = EntityStatus.Available)
        {
            return this.Entities.Where(e => e.Status == entityStatus).AsEnumerable();
        }

        public T Insert(T entity)
        {
            entity.Status = EntityStatus.Available;

            try
            {
                EntityEntry<T> entityEntry = this.Entities.Add(entity);
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Update(T entity)
        {
            this.Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
