using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Queries;

namespace Data.Repositories
{
    [AutoRegister]
    public class InMemoryRepository<T> : IInMemoryRepository<T> where T : class
    {
        private readonly List<T> _items = new List<T>();

        public void Create(T item) => _items.Add(item);

        public void Update(T item)
        {
            var idProp = typeof(T).GetProperty("Id");
            if (idProp == null) return;
            var id = idProp.GetValue(item).ToString();
            Delete(id);
            _items.Add(item);
        }

        public void Delete(string id)
        {
            var item = _items.FirstOrDefault(i =>
                typeof(T).GetProperty("Id")?.GetValue(i).ToString() == id);
            if (item != null) _items.Remove(item);
        }

        public T GetById(string id) =>
            _items.FirstOrDefault(i =>
                typeof(T).GetProperty("Id")?.GetValue(i).ToString() == id);

        public IEnumerable<T> GetAll(Func<T, bool> filter = null) =>
            filter == null ? _items : _items.Where(filter);
    }
}