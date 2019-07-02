using System;
using Raven.Client.Documents;

namespace DCNC.Service.Database
{
    public class DocumentStoreHolder
    {
        static readonly Lazy<IDocumentStore> _store = new Lazy<IDocumentStore>(CreateDocumentStore);

        static IDocumentStore CreateDocumentStore()
        {
            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "DcncDb"
            };

            documentStore.Initialize();
            return documentStore;
        }

        public static IDocumentStore Store => _store.Value;
    }
}