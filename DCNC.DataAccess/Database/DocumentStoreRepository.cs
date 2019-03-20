using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace DCNC.DataAccess.Database
{
    public class DocumentStoreRepository
    {
        public void Save<T>(T documentToSave)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(documentToSave);
                session.SaveChanges();
            }
        }

        public void MassSave<T>(List<T> objectsToSave)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                objectsToSave.ForEach(item => session.Store(item));
                session.SaveChanges();
            }
        }
    }
}