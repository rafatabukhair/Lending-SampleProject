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
    public class Repository<T> : IRepository<T> where T : IdObject
    {
        private readonly IDocumentSession _documentSession;

        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Save(T entity)
        {
            _documentSession.Store(entity);
        }

        public void Delete(T entity)
        {
            _documentSession.Delete(entity);
        }

        public T Get(Guid id)
        {
            return _documentSession.Load<T>(id.ToString());
        }

        protected void DeleteAll<TIndex>() where TIndex : AbstractIndexCreationTask, new()
        {
            var indexName = new TIndex().IndexName;

            _documentSession.Advanced.DocumentStore.Operations.Send(new Raven.Client.Documents.Operations.DeleteByQueryOperation(
                new IndexQuery { Query = "from index '" + indexName + "'"}, null));

            _documentSession.SaveChanges();
        }
    }
}