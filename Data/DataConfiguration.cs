using System.Reflection;
using BusinessEntities;
using Common;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Conventions;
using SimpleInjector;

namespace Data
{
    public class DataConfiguration
    {
        public static void Initialize(Container container, Lifestyle lifestyle, bool createIndexes = true)
        {
            var assembly = typeof(DataConfiguration).Assembly;

            container.RegisterSingleton<IListTypeLookup<Assembly>, ListTypeLookup<Assembly>>();

            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, typeof(User).Assembly);
            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, assembly);

            InitializeAssemblyInstancesService.Initialize(container, lifestyle, assembly);
            container.RegisterSingleton(() => InitializeDocumentStore(assembly, createIndexes));

            container.Register(() =>
                               {
                                   var session = container.GetInstance<IDocumentStore>().OpenSession();
                                   session.Advanced.MaxNumberOfRequestsPerSession = 5000;
                                   return session;
                               }, lifestyle);
        }

        private static IDocumentStore InitializeDocumentStore(Assembly assembly, bool createIndexes)
        {
            var documentStore = new DocumentStore
                                {
                                    Urls = new[] { "http://localhost:8080/" },
                                    Database = "SampleProject",
                                    Conventions =
                                    {
                                        UseOptimisticConcurrency = true,
                                        SaveEnumsAsIntegers = true
                                    }
                                };

            documentStore.Initialize();

            if (createIndexes)
            {
                IndexCreation.CreateIndexes(assembly, documentStore);
            }

            return documentStore;
        }
    }
}