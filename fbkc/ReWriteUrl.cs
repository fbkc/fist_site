using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace fbkc
{
    public class ReWriteUrl : IRouteHandler
    {
        public string UrlRote
        {
            get;
            private set;
        }
        public ReWriteUrl(string sUrlRote)
        {
            UrlRote = sUrlRote;
        }
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return BuildManager.CreateInstanceFromVirtualPath(UrlRote, typeof(IHttpHandler)) as IHttpHandler;
        }
    }
}