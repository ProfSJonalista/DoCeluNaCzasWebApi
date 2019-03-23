using Microsoft.AspNet.Identity.Owin;
using Raven.Client.Documents.Session;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DoCeluNaCzasWebApi.Controllers
{
    public abstract class RavenController : ApiController
    {
        public bool AutoSave { get; set; }
        private IAsyncDocumentSession dbSession;

        public IAsyncDocumentSession RavenSession
        {
            get
            {
                dbSession = dbSession ?? Request.GetOwinContext().Get<IAsyncDocumentSession>();

                return dbSession;
            }

            set => dbSession = value;
        }

        public RavenController()
        {
            this.AutoSave = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (RavenSession != null)
                {
                    using (RavenSession)
                    {
                        if (AutoSave)
                            RavenSession.SaveChangesAsync();
                        RavenSession.Dispose();
                        //RavenSession = null;
                    }
                }
            }

            base.Dispose(disposing);
        }

        //private IAsyncDocumentSession dbSession;

        //public RavenController()
        //{
        //}

        ///// <summary>
        ///// Gets the Raven document session for this request.
        ///// </summary>
        //public IAsyncDocumentSession DbSession
        //{
        //    get
        //    {
        //        if (this.dbSession == null)
        //        {
        //            this.dbSession = HttpContext.GetOwinContext().Get<IAsyncDocumentSession>();
        //        }

        //        return dbSession;
        //    }
        //}

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (!filterContext.IsChildAction)
        //    {
        //        using (DbSession)
        //        {
        //            if (filterContext.Exception == null && this.DbSession != null && this.DbSession.Advanced.HasChanges)
        //            {
        //                var saveTask = this.DbSession.SaveChangesAsync();

        //                // Tell the task we don't need to invoke on MVC's SynchronizationContext. 
        //                // Otherwise we can end up with deadlocks. See http://code.jonwagner.com/2012/09/04/deadlock-asyncawait-is-not-task-wait/
        //                saveTask.ConfigureAwait(continueOnCapturedContext: false);
        //                saveTask.Wait(TimeSpan.FromSeconds(10));
        //            }
        //        }
        //    }
        //}
    }
}