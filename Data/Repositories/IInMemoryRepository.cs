using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IInMemoryRepository<T>
    {
        void Create(T item);
        void Update(T item);
        void Delete(string id);
        T GetById(string id);
        IEnumerable<T> GetAll(Func<T, bool> filter = null);
    }
}