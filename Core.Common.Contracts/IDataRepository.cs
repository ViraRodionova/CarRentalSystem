using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Contracts
{
    /*
     *  Если есть инт-с с Generics, то он создает всегда инт-с без Generics
     */
    public interface IDataRepository
    {

    }

    public interface IDataRepository<T> : IDataRepository
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);

        void Remove(T entity);

        void Remove(int id);

        T Update(T entity);

        IEnumerable<T> Get();

        T Get(int id);
    }
}
