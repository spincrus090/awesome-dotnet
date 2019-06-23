using SB.Enums;
using SB.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SB.Core.Repository.Interface
{
    public interface IAuditableRepository<T> where T : AuditableEntity
    {
        IEnumerable<T> GetAll(EntityStatus entityStatus = EntityStatus.Available);
        T Get(Guid id, EntityStatus entityStatus = EntityStatus.Available);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available);
        T Insert(T entity);
        T Update(T entity);
        T Delete(T entity, bool isHardDelete = false);
    }
}
