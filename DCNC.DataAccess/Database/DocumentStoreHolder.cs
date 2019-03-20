using Raven.Client.Documents;
using System;

namespace DCNC.DataAccess.Database
{
    public class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> _store = new Lazy<IDocumentStore>(CreateDocumentStore);

        private static IDocumentStore CreateDocumentStore()
        {
            const string serverUrl = "http://127.0.0.1:8080";
            const string databaseName = "DcncDb";

            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] { serverUrl },
                Database = databaseName
            };

            documentStore.Initialize();
            return documentStore;
        }

        public static IDocumentStore Store
        {
            get { return _store.Value; }
        }
    }
}