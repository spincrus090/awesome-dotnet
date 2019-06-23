using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SB.Core.Repository.Interface
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T Get(Guid id);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Insert(T entity);

        T Update(T entity);

        T Delete(T entity);
    }
}
